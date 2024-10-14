
function validation(frmName) {
    var msg = '';
    var valid = true;
    if (frmName == 'saveUser') {

        if ($("#name").val() == '') {
            msg = msg + "\n" + "Please enter Name"
            valid = false;
        }
        if ($("#selectCategory").val() == 0) {
            msg = msg + "\n" + " Select Category";
            valid = false;
        }

        if (valid) {
            save()

        }
        else {
            alert(msg);
            location.reload();
        }
    }
    if (frmName == 'saveCategory') {

        if ($("#name").val() == '') {
            msg = msg + "\n" + "Please enter CategoryName"
            valid = false;
        }
        
        if (valid) {
            saveCategory()

        }
        else {
            alert(msg);
            location.reload();
        }
    }
}

function save() {
    var fdata = new FormData();
    var name = $("#name").val();
    fdata.append("name", name)
    var id = $("#id").val();
    fdata.append("id", id)

    $('#selectCategory').each(function () {
        if ($(this).attr('id') !== undefined) {
            fdata.append($(this).attr('id'), $('#' + $(this).attr('id') + ' option:selected').val());
        }
    });
    $.ajax({
        url: '/Product/Save',
        data: fdata,
        type: "POST",
        processData: false,
        contentType:false,
        success: function (data) {
            if (data.success) {
                alert(data.massage);
                location.href = "/Product/Index";
            }
        },
        complete: function () {
            
        },
        error: function () {

        }
    })
}


function Delete(id , statusid) {
    
    $.ajax({
        url: '/Product/Delete',
        type: "POST",
        data: { Id: id, StatusId: statusid },
        success: function (data) {
            if (data.success) {
                alert(data.massage);
                location.reload();
            }
        },
        complete: function () {

        },
        error: function () {

        }
    })
}


function GetCategory(id) {

    $.ajax({
        url: '/Product/GetCategory',
        type: "GET",
        data: {},
        success: function (data) {
            
            if (data.success) {

                var list = data.data;
                for (item in list)
                {
                    if (list[item].categoryId == id) {
                        $("#selectCategory").append('<option selected value="' + list[item].categoryId + '">' + list[item].categoryName + '</option>')
                    }
                    else {
                    $("#selectCategory").append('<option value="' + list[item].categoryId + '">' + list[item].categoryName +'</option>')

                    }
                }
                
            }
        },
        complete: function () {

        },
        error: function () {

        }
    })
}



function saveCategory() {
    var fdata = new FormData();
    var name = $("#name").val();
    var id = $("#id").val();
    fdata.append("id", id)
    fdata.append("name", name)
    $.ajax({
        url: '/Category/Save',
        data: fdata,
        type: "POST",
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                alert(data.massage);
                location.href = "/Category/Index";
            }
        },
        complete: function () {

        },
        error: function () {

        }
    })
}

function DeleteCategory(id, statusid) {

    $.ajax({
        url: '/Category/Delete',
        type: "POST",
        data: { Id: id, StatusId: statusid },
        success: function (data) {
            if (data.success) {
                alert(data.massage);
                location.reload();
            }
        },
        complete: function () {

        },
        error: function () {

        }
    })
}
