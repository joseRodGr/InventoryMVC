let ConfirmDelete = (itemId, itemTitle, itemName) => {
    $('#hiddenId').val(itemId);
    $('#itemTitle').text(itemTitle);
    $('#itemName').text(itemName);
    $('#deleteModal').modal('show');
};

let Delete = (controllerName, actionName) => {

    const itemId = $('#hiddenId').val();

    $.ajax({
        type: 'POST',
        url: '/' + controllerName + '/' + actionName,
        data: { id: itemId },
        success: () => {
            $('#deleteModal').modal('hide');
            $('#row_' + itemId).remove();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#deleteModal').modal('hide');
            //console.log(XMLHttpRequest);
            //if (XMLHttpRequest.status !== 500) {
            document.write(XMLHttpRequest.responseText);
            //}
        }

    });
};