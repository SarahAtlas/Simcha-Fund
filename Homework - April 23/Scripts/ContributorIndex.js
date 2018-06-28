$(function () {

    $("#newContributor").on('click', function () {
        $(".newContributor").modal();
    });

    $(".depositButton").on('click', function () {
        const contributorId = $(this).data('contributorId');
        $('#contributorId').val(contributorId);

        const firstName = $(this).data('firstName');
        const lastName = $(this).data('lastName');
        const name = firstName + " " + lastName;
        $("#contributorName").text(name);

        $(".deposit").modal();
    });

    $("#search").on('keyup', function () {
        const text = $(this).val();
        $("table tr:gt(0)").each(function () {
            const tr = $(this);
            const name = tr.find('td:eq(1)').text();
            if (name.toLowerCase().indexOf(text.toLowerCase()) !== -1) {
                tr.show();
            } else {
                tr.hide();
            }
        });
    });

    $("#clear").on('click', function () {
        $("#search").val('');
        $("tr").show();
    });

    $(".editContributor").on('click', function () {
        console.log("Hello");
        const id = $(this).data('id');
        const firstName = $(this).data('firstName');
        const lastName = $(this).data('lastName');
        const cell = $(this).data('cell');
        const alwaysInclude = $(this).data('alwaysInclude');
        const date = $(this).data('date');

        $("#edit_firstName").val(firstName);
        $("#edit_lastName").val(lastName);
        $("#edit_cellNumber").val(cell);
        $("#edit_alwaysInclude").prop('checked', alwaysInclude === "True");
        $("#edit_date").val(date);
        $("#edit_id").val(id);

        $(".edit").modal();
    });
});
