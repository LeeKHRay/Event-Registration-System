(function () {
    const $form = $("#event-form");
    const $dropArea = $("#drop-area");
    const $imagesPreview = $("#images-preview");
    const $imageUploader = $("#FormImages");

    const validImageTypes = ["image/jpg", "image/jpeg", "image/png"];
    const dataTransfer = new DataTransfer();

    $imageUploader.prop("accept", validImageTypes.join(","));

    $(document).on("dragenter dragover", "#drop-area", e => {
        e.preventDefault();

        $dropArea.addClass("highlight");
    });

    $(document).on("dragleave drop", "#drop-area", e => {
        e.preventDefault();

        $dropArea.removeClass("highlight");
    });

    const selectFiles = files => {
        for (const file of files) {
            if (validImageTypes.includes(file.type)) { // check if the type of each selected image is valid
                const $imgPreview = $("<img>", {
                    src: URL.createObjectURL(file),
                    draggable: false
                });

                $imagesPreview.append($('<div class="img-preview">').append($imgPreview));
                dataTransfer.items.add(file);
            }
        }
    };

    // select images by drag and drop
    $(document).on("drop", "#drop-area", ({ originalEvent: e }) => {
        selectFiles(e.dataTransfer.files);
    });

    // select images from file manager
    $(document).on("change", "#FormImages", e => {
        e.preventDefault();
        selectFiles(e.target.files);
    });

    // click on uploaded image's container
    $(document).on("click", ".uploaded-img", ({ currentTarget }) => {
        $(currentTarget).children("img").toggleClass("unselected");
    });

    // click on selected image's container
    $(document).on("click", ".img-preview", ({ currentTarget }) => {
        const $target = $(currentTarget);

        URL.revokeObjectURL($target.children("img").prop("src"))
        dataTransfer.items.remove($target.index(".img-preview"));
        $target.remove();
    });

    $(document).on("click", "#submit-btn", e => {
        e.preventDefault();

        $imageUploader.prop("files", dataTransfer.files);

        $("img[data-img-id]:not(.unselected)").each((i, img) => {
            $form.append(`<input type="hidden" name="UploadedImageIds[${i}]" value="${img.dataset["imgId"]}">`);
        });

        $form.trigger("submit");
    });
})();