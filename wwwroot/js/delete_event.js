(function () {
    $(document).on("click", "#delete-btn", e => {
        e.preventDefault();

        if (confirm("Are you sure you want to delete this event?")) {
            $.ajax({
                url: `/api/events/${$(e.target).data("id")}`,
                method: "DELETE",
                headers: {
                    RequestVerificationToken: $("input[name=__RequestVerificationToken]").val() // ASP.NET Core does not parse JSON to look for token
                }
            })
            .done(() => {
                location.href = "/Events/MyEvents";
            });
        }
    });
})();