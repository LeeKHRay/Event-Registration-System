(function () {
    const $organizationFields = $(".organization-field");

    const toggleFields = role => {
        if (role == "GeneralUser") {
            $organizationFields.addClass("d-none");
        }
        else if (role == "OrganizationUser") {
            $organizationFields.removeClass("d-none");
        }
    }

    toggleFields($("input:radio[name='Input.Role']:checked").val());

    $(document).on("change", "input:radio[name='Input.Role']:checked", ({ currentTarget }) => {
        const role = $(currentTarget).val();
        toggleFields(role);
    });
})();