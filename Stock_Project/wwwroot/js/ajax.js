function updateDropdownStoreValue(storeId) {
    $.ajax({
        url: '/StoreItem/GetStoreNameByID',
        type: 'GET',
        data: { id: storeId },
        success: function (data) {
            $('#storeIDInputForDlt').val(storeId);
            $('#StoredropdownButton').empty();
            if (data) {
                $('#StoredropdownButton').html(data[0]);
            } else {
                console.log("No Items Found");
            }
        },
        error: function () {
            console.log('Error fetching items');
        }
    })
}
function updateDropdownStoreValueCreate(storeId) {
    $.ajax({
        url: '/StoreItem/GetStoreNameByID',
        type: 'GET',
        data: { id: storeId },
        success: function (data) {
            $('#storeIDInput').val(storeId);
            $('#StoredropdownButtonCreate').empty();
            if (data) {
                $('#StoredropdownButtonCreate').html(data[0]);
            } else {
                console.log("No Items Found");
            }
        },
        error: function () {
            console.log('Error fetching items');
        }
    })
}

function updateDropdownItemValue(itemId) {
    $.ajax({
        url: '/StoreItem/GetItemNameByID',
        type: 'GET',
        data: { id: itemId },
        success: function (data) {
            $('#itemIDInputForDlt').val(itemId);
            $('#itemDropdownButton').empty();
            if (data) {
                $('#itemDropdownButton').html(data[0]);
            } else {
                console.log("No Items Found");
            }
        },
        error: function () {
            console.log('Error fetching items');
        }
    })
}
function updateDropdownItemValueCreate(itemId) {
    $.ajax({
        url: '/StoreItem/GetItemNameByID',
        type: 'GET',
        data: { id: itemId },
        success: function (data) {
            $('#itemIDInput').val(itemId);
            $('#itemDropdownButtonCreate').empty();
            if (data) {
                $('#itemDropdownButtonCreate').html(data[0]);
            } else {
                console.log("No Items Found");
            }
        },
        error: function () {
            console.log('Error fetching items');
        }
    })
}

function updateQuantityValue(itemId,storeId) {
    $.ajax({
        url: '/StoreItem/GetQuantityByStoreIdAndItemId',
        type: 'GET',
        data: {
            storeId: storeId,
            itemId: itemId
        },
        success: function (data) {
            $('#QuantityValue').empty();
            if (data) {
                $('#QuantityValue').val(data[0].quantity);
            } else {
                console.log("No Items Found");
            }
        },
        error: function () {
            console.log('Error fetching items');
        }
    })
}

function updateItemsDropdown(storeId) {
    $.ajax({
        url: '/StoreItem/GetItemByStore',
        type: 'GET',
        data: { id: storeId },
        success: function (data) { 
            $('#itemDropdown').empty();
            if (data && data.length) {
                $('#itemDropdownButton').html("Select Item");
                $('#QuantityValue').val("Select An Item");
                for (var i = 0; i < data.length; i++) {
                    (function (index) {
                        var listItem = $('<li>').append(
                            $('<a>', {
                                class: 'dropdown-item',
                                href: '#',
                                text: data[index].name,
                                click: function () {
                                    updateDropdownItemValue(data[index].itemId);
                                    updateQuantityValue(data[index].itemId, data[index].storeId)
                                }
                            })
                        );
                        $('#itemDropdown').append(listItem);
                    })(i)
                }
            } else {
                $('#QuantityValue').val("Select An Item");
                $('#itemDropdownButton').html("No Items");
            }   
        },
        error: function () {
            console.log('Error fetching items');
        }
    })
}

function deleteStoreItem() {
    var formData = {
        StoreID: $('#storeIDInputForDlt').val(),
        ItemID: $('#itemIDInputForDlt').val(),
        Quantity: $('.deletedQuantity').val()
    };
    $.ajax({
        url: '/StoreItem/CheckIfExists',
        type: 'POST',
        data: formData,
        success: function (result) {
            if (result.exists) {
                updateDeletedQuantity(formData);
            } else {
                //deleteRecord(formData);
            }
        },
        error: function () {
            console.log("AJAX Error");
        }
    });
}
function updateDeletedQuantity(formData) {
    $.ajax({
        url: '/StoreItem/GetQuantityByStoreIdAndItemId',
        type: 'GET',
        data: {
            storeId: formData.StoreID,
            itemId: formData.ItemID
        },
        success: function (data) {
            if (data) {
                var store_id = formData.StoreID;
                var item_id = formData.ItemID;
                var deletedQuantity = formData.Quantity;
                var oldQuantity = data[0].quantity;
                if (oldQuantity > deletedQuantity) {
                    //I Should Update the Quantity
                    var updatedQuantity = parseInt(oldQuantity) - parseInt(deletedQuantity);
                    var updatedStoreItem = {
                        StoreID: formData.StoreID,
                        ItemID: formData.ItemID,
                        Quantity: updatedQuantity
                    };
                    $.ajax({
                        url: '/StoreItem/Edit?store_id=' + store_id + '&item_id=' + item_id,
                        type: 'POST',
                        data: updatedStoreItem,
                        success: function (result) {
                            if (result.success) {
                                alert("Update Done Successfully");
                                console.log("Update Done Successfully");
                            }
                            else {
                                alert("Error in update");
                                console.log("Error in update");
                            }
                        },
                        error: function () {
                            console.log("AJAX Error");
                        }
                    });
                }
                else if (oldQuantity == deletedQuantity)
                {
                    //I Should Delete The Record
                    $.ajax({
                        url: '/StoreItem/DeleteConfirmation',
                        type: 'POST',
                        data: {
                            store_id: formData.StoreID,
                            item_id: formData.ItemID
                        },
                        success: function (result) {
                            if (result.success) {
                                alert("Delete Done Successfully");
                                console.log("Delete Done Successfully");
                            }
                            else {
                                alert("Error in delete");
                                console.log("Error in delete");
                            }
                        },
                        error: function () {
                            console.log("AJAX Error");
                        }
                    });
                }
                else if (oldQuantity < deletedQuantity)
                {
                    alert("Deleted Quantity Greater Than The Actual Quantity");
                    console.log("Deleted Quantity Greater Than The Actual Quantity");
                }
            } else {
                console.log('Error in fetching the data');
            }

        },
        error: function () {
            console.log('AJAX Error');
        }
    });
}
    
function submitStoreItemForm(event) {
    event.preventDefault();
    var formData = {
        StoreID : $('#storeIDInput').val(),
        ItemID : $('#itemIDInput').val(),
        Quantity : $('.quantity').val()
    };
    console.log(formData);
    $.ajax({
        url: '/StoreItem/CheckIfExists',
        type: 'POST',
        data: formData,
        success: function (result) {
            if (result.exists) {
                updateQuantity(formData);
            } else {
                createNewRecord(formData);
            }
        },
        error: function () {
            console.log("AJAX Error");
        }
    });
}
function updateQuantity(formData) {
    $.ajax({
        url: '/StoreItem/GetQuantityByStoreIdAndItemId',
        type: 'GET',
        data: {
            storeId: formData.StoreID,
            itemId: formData.ItemID
        },
        success: function (data) {
            $('#QuantityValue').empty();
            if (data) {
                var store_id = formData.StoreID;
                var item_id = formData.ItemID;
                var newQuantity = formData.Quantity;
                var oldQuantity = data[0].quantity;
                var updatedQuantity = parseInt(oldQuantity) + parseInt(newQuantity);
                var updatedStoreItem = {
                    StoreID: formData.StoreID,
                    ItemID: formData.ItemID,
                    Quantity: updatedQuantity
                };
                console.log(updatedStoreItem);
                $.ajax({
                    url: '/StoreItem/Edit?store_id=' + store_id + '&item_id=' + item_id,
                    type: 'POST',
                    data: updatedStoreItem,
                    success: function (result) {
                        if (result.success) {
                            alert("Update Done Successfully");
                            console.log("Update Done Successfully");
                        } else {
                            alert("Error in update");
                            console.log("Error in update");
                        }
                    },
                    error: function () {
                        console.log("AJAX Error");
                        console.log(error);
                    }
                });

            } else {
                console.log("Error in getting the old quantity");
            }
        },
        error: function () {
            console.log('Error fetching items');
        }
    });
}
function createNewRecord(formData) {
    $.ajax({
        url: '/StoreItem/Create',
        type: 'POST',
        data: formData,
        success: function (result) {
            if (result.success) {
                alert("Record Added Successfully");
                console.log("Record Added Successfully");
            } else {
                alert("Error in creation");
                console.log("Error in creation")
            }
        },
        error: function () {
            console.log("AJAX Error");
        }
    });
}


$(document).ready(function () {
    $('#addButton').click(function () {
        $('#detailsStoreItem').toggle();
        $('#hiddenImage').toggle();
        $('#hiddenDiv').toggle();
        $('#StoredropdownButton').html("Select Store");
        $('#itemDropdownButton').html("Store Items");
        $('#QuantityValue').val("Select An Item");
        $('#StoredropdownButtonCreate').html("Select Store");
        $('#itemDropdownButtonCreate').html("Select Item");
        $('.quantity').val("0");
    });
    $('#addRequestBtn').click(function () {
        $('#detailsStoreItem').toggle();
        $('#hiddenImage').toggle();
        $('#hiddenDiv').toggle();
        var isHiddenDeductQuantityVisible = $('#hiddenDeductQuantity').is(":visible");
        if (isHiddenDeductQuantityVisible) {
            ('#hiddenDeductQuantity').toggle();
        }
    });
    $('#DltButton').click(function () {
        $('#hiddenDeductQuantity').toggle();
    });
    $('#confirmDltButton').click(function () {
        deleteStoreItem();
        $('#hiddenDeductQuantity').toggle();
        $('#StoredropdownButton').html("Select Store");
        $('#itemDropdownButton').html("Store Items");
        $('#QuantityValue').val("Select An Item");
    });
    $('#BackBtn').click(function () {
        $('#detailsStoreItem').toggle();
        $('#hiddenImage').toggle();
        $('#hiddenDiv').toggle();
        $('#hiddenDeductQuantity').toggle();
    });
    $('#cancelButton').click(function () {
        $('#hiddenDeductQuantity').toggle();
        $('#StoredropdownButton').html("Select Store");
        $('#itemDropdownButton').html("Store Items");
        $('#QuantityValue').val("Select An Item");
    });
})