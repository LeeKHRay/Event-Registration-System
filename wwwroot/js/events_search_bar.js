(function () {
    const requestUrl = window.requestUrl;
    const currentUrl = location.origin + location.pathname;
    const $form = $("#events-search-form");
    const $events = $("#events");
    const $eventNum = $("#event-num");
    let displayType = window.displayType;

    const getEvents = () => {
        const fieldNames = new Set(["CategoryId", "Name", "SortBy"]);
        const queryString = $form.serializeArray().reduce((pairs, { name, value }) => {
            if (fieldNames.has(name)) {
                pairs.push(`${name}=${value}`);
            }
            return pairs;
        }, []).join("&");
        const apiUrl = `${requestUrl}?${queryString}&displayType=${displayType}`;

        $.get(apiUrl)
            .done(res => {
                // if it is list view, the returned html need to be wrapped in div for find() to find .event
                const eventNum = $(res).wrapAll("<div></div>").parent().find(".event").length;

                $eventNum.html(`${eventNum} Event${eventNum == 1 ? "" : "s"}`);
                $events.html(res);

                history.replaceState("", "", `${currentUrl}?${queryString}`);
            })
            .fail(res => {
                console.log(res.responseText);
            });
    }

    $(document).on("submit", "#events-search-form", e => {
        e.preventDefault()
        getEvents();
    });

    $(document).on("click", "#search-events-btn", e => {
        getEvents();
    });

    $(document).on("click", "#grid-view-btn", e => {
        if (displayType == "Grid") {
            return;
        }

        displayType = "Grid";
        getEvents();
    });

    $(document).on("click", "#list-view-btn", e => {
        if (displayType == "List") {
            return;
        }

        displayType = "List";
        getEvents();
    });


    const colors = ["blue", "red", "green"];

    for (const color of colors) {
        $(document).on("click", `#${color}-btn`, e => {
            e.preventDefault();

            $.ajax({
                url: "/api/users/preference",
                method: "POST",
                headers: {
                    RequestVerificationToken: $("input[name=__RequestVerificationToken]").val() // ASP.NET Core does not parse JSON to look for token
                }, 
                contentType: 'application/json',
                data: JSON.stringify({ color })
            })
            .done(res => {
                const $events = $("#events");

                $events.removeClass(colors)
                $events.addClass(res.color)

                for (const color of colors) {
                    $(`#${color}-btn`).removeClass("selected");
                }
                $(`#${color}-btn`).addClass("selected");
            });
        });
    }
})();