<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="CounterpartyInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.CounterpartyInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">

    
                <form id="Form1" runat="server">
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <label>
                    Name:
                    <input type="text" name="name" id="name" class="round input-form-textbox" /></label>
                    <button type="submit" id="LoadRecordsButton" class="round blue button-submit">
                    Load records</button>
            </div>
        </div>
    </div>
    <div class="table-container">
        <div id="TableContainer">
        </div>
    </div>      
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="Div1">
                <label>
                    Name:
                    <input type="text" name="name2" id="name2" class="round input-form-textbox" /></label>
                    <button type="submit" id="LoadRecordsButton2" class="round blue button-submit">
                    Load records</button>
            </div>
        </div>
    </div>  
                </form>
     <div class="table-container">
         <div id="TableContainer2">
        </div>
    </div>
    <div id="select-product" class="table-container" style="display:none; "></div>
    <script type="text/javascript">

        $(document).ready(function () {

            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Static: Counterparties',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'SNAME ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetByFilter") %>'
                    <% if (Writable) {
                        Response.Write(", createAction: '" + Page.ResolveClientUrl("CounterpartyInfo.aspx/Create") + "'");
                        Response.Write(", updateAction: '" + Page.ResolveClientUrl("CounterpartyInfo.aspx/Update") + "'");
                    } %>
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    //CHILD TABLE DEFINITION FOR "CTPY"
                    LIMIT: {
                        title: 'Limit',
                        width: '3%',
                        sorting: false,
                        create: false,
                        edit: false,
                        display: function (CTPYData) {
                            //Create an image that will be used to open child table
                            var $img = $('<img src="../images/list_metro.png" title="Edit limit configuration" />');
                            //Open child table when user clicks the image
                            $img.click(function () {
                                $('#TableContainer').jtable('openChildTable',
                                    $img.closest('tr'), //Parent row
                                    {
                                    title: CTPYData.record.SNAME + ' - Limit',
                                    actions: {
                                        listAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetCtpyLimitByCtpyID?ID=' + CTPYData.record.ID + '") %>'
                                        <% if (Writable) {
                                            Response.Write(", updateAction: '" + Page.ResolveClientUrl("CounterpartyInfo.aspx/UpdateCtpyLimit") + "'");
                                        } %>
                                    },
                                    fields: {
                                        CTPY_ID: {
                                            type: 'hidden',
                                            defaultValue: CTPYData.record.ID
                                        },
                                        ID: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        LIMIT_ID: {
                                            title: 'Limit',
                                            width: '30%',
                                            options: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetLimitOptions") %>',
                                            inputClass: 'validate[required]',
                                            key: true,
                                            edit: false
                                        },
                                        AMOUNT: {
                                            title: 'Amount',
                                            width: '20%',
                                            type: 'number',
                                            inputClass: 'validate[required]',
                                            listClass: 'right'
                                        },
                                        EXPIRE_DATE: {
                                            title: 'Expire date',
                                            width: '30%',
                                            type: 'date',
                                            displayFormat: 'dd-M-yy',
                                            inputClass: 'validate[required]'
                                        },
                                        FLAG_CONTROL: {
                                            title: 'Control',
                                            width: '12%',
                                            type: 'checkbox',
                                            values: { 'false': 'No-Restriction', 'true': 'Control' },
                                            defaultValue: 'true',
                                            formText: 'Control'
                                        }
                                    },
                                    //Initialize validation logic when a form is created
                                    formCreated: function (event, data) {

                                        data.form.find('input[name="AMOUNT"]').number(true);
                                    },
                                    rowUpdated: function (event, data){
                                        $('#LoadRecordsButton2').click();
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                            });
                            //Return image to show on the person row
                            return $img;
                        }
                    },
                    //CHILD TABLE DEFINITION FOR "CSA Agreement"
                    CSA: {
                        title: 'CSA',
                        width: '3%',
                        sorting: false,
                        create: false,
                        edit: false,
                        display: function (CTPYData) {
                            //Create an image that will be used to open child table
                            var $img = $('<img src="../images/list_metro.png" title="Edit CSA" />');
                            //Open child table when user clicks the image
                            $img.click(function () {
                                $('#TableContainer').jtable('openChildTable',
                                    $img.closest('tr'), //Parent row
                                    {
                                    title: CTPYData.record.SNAME + ' - CSA Agreement',
                                    actions: {
                                        listAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetCSAByCtpyID?ID=' + CTPYData.record.ID + '") %>'
                                        <% if (Writable) {
                                            Response.Write(", createAction: '" + Page.ResolveClientUrl("CounterpartyInfo.aspx/CreateCSA") + "'");
                                            Response.Write(", updateAction: '" + Page.ResolveClientUrl("CounterpartyInfo.aspx/UpdateCSA") + "'");
                                        } %>
                                    },
                                    fields: {
                                        ID: {
                                            type: 'hidden',
                                            defaultValue: CTPYData.record.ID
                                        },
                                        CSA_TYPE_ID: {
                                            title: 'Type',
                                            options: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetCSATypeOptions") %>',
                                            inputClass: 'validate[required]'
                                        },
                                        PRODUCTS: {
                                            title: 'Product',
                                            create: false,
                                            edit: false
                                        },
                                        ISACTIVE: {
                                            title: 'Status',
                                            type: 'checkbox',
                                            values: { 'false': 'Inactive', 'true': 'Active' },
                                            formText: 'Active',
                                            defaultValue: 'true'
                                        },
                                        CustomAction2: {
                                            title: 'Select Product',
                                            width: '3%',
                                            sorting: false,
                                            create: false,
                                            edit: false,
                                            list: true,
//                                            display: function (data) {
//                                                if (data.record) {
//                                                    return '<button title="Edit Record" class="jtable-command-button jtable-edit-command-button" onclick="selectProduct(\'' + data.record.ID + '\'); return false;"><span>Select Product</span></button>';
//                                                }
//                                                else {return '';}
//                                            }
                                            display: function (CSAData) {
                                                //Create an image that will be used to open child table
                                                var $img = $('<img src="../images/tab-dashboard.png" title="Select Product" />');
                                                //Open child table when user clicks the image
                                                $img.click(function () {                                              
                                                    $('#select-product').jtable({
                                                        title: 'Select Product',
                                                        paging: false,
                                                        sorting: false,
                                                        actions: {
                                                            listAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetCSAProducts?ID=' + CSAData.record.ID + '") %>',
                                                            createAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/CreateCSAProduct") %>',
                                                            deleteAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/DeleteCSAProduct") %>'
                                                        },
                                                        fields: {
                                                            CSA_AGREEMENT_ID: {
                                                                key: true,
                                                                create: true,
                                                                edit: true,
                                                                type: 'hidden',
                                                                defaultValue: CSAData.record.ID
                                                            },
                                                            PRODUCT_ID: {
                                                                key2: true,
                                                                title: 'Product',
                                                                options: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetProductOptions") %>'
                                                            }
                                                        },
                                                        closeRequested: function (event, data){
                                                            $('#select-product').jtable('destroy');
                                                        }
                                                    });

                                                    $('#select-product').jtable('reload');
                                                    $('#select-product').show();

                                                    $("#select-product").dialog({
                                                        title: 'Select Product',
                                                        height: 300,
                                                        width: 450,
                                                        modal: true
                                                    });
                                                    
                                                });

                                                //Return image to show on the person row
                                                return $img;
                                            }
                                        }
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                            });
                            //Return image to show on the person row
                            return $img;
                        }
                    },
                    USERCODE: {
                        title: 'OPICS ID',
                        width: '7%'
                    },
                    SNAME: {
                        title: 'Short Name',
                        width: '10%'
                    },
                    FNAME: {
                        title: 'Full Name',
                        width: '30%'
                    },
                    TBMA_NAME: {
                        title: 'TBMA Name',
                        width: '15%'
                    },
                    COUNTRY_ID: {
                        title: 'Country',
                        options: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetCountryOptions") %>',
                        width: '7%'
                    },
                    BUSINESS: {
                        title: 'Business',
                        width: '10%'
                    },
                    GROUP_CTPY_ID: {
                        title: 'Group',
                        options: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetCPTYGroupOptions") %>',
                        width: '10%'
                    },
                    RATE: {
                        title: 'Rate',
                        width: '10%'
                    },
                    OUTLOOK: {
                        title: 'Outlook',
                        width: '10%'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        width: '10%',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        formText: 'Active',
                        defaultValue: 'true'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="USERCODE"]').addClass('validate[required]');
                    data.form.find('input[name="SNAME"]').addClass('validate[required]');
                    data.form.find('input[name="FNAME"]').addClass('validate[required]');
                    data.form.find('input[name="BUSINESS"]').addClass('validate[required]');
                    data.form.find('input[name="TBMA_NAME"]').addClass('validate[required]');
                    data.form.find('input[name="USERCODE"]').autocomplete({
                        minLength: 2,
                        source: function (request, response) {
                            var jsdata = { name_startsWith: request.term };

                            $.ajax({
                                type: "POST",
                                url: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetOPICNameByFilter") %>',
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(jsdata),
                                success: function (data) {
                                    //alert(data);
                                    response($.map(data.d.Records, function (item) {
                                        return {
                                            label: item.CMNE,
                                            value: item.CNO,
                                            name: item.SN,
                                            ctype: item.CTYPE,
                                            ccode: item.CCODE
                                        }
                                    }));
                                }
                            });
                        },
                        select: function (event, ui) {
                            //alert(ui.item.label);
                            $('input[name="SNAME"]').val(ui.item.label);
                            $('input[name="FNAME"]').val(ui.item.name);
                            var bus = "";
                            if (ui.item.ctype == "B") bus = "Bank";
                            else if (ui.item.ctype == "C") bus = "Corporate";
                            else if (ui.item.ctype == "O") bus = "Office";
                            else bus = "";
                            $('input[name="BUSINESS"]').val(bus);
                            $("#Edit-COUNTRY_ID option:contains('" + ui.item.ccode + "')").attr('selected', true);
                        },
                        open: function () {
                            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                        },
                        close: function () {
                            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                        }
                    });
                    data.form.validationEngine();
                },
                //Validate form when it is being submitted
                formSubmitting: function (event, data) {

                    return data.form.validationEngine('validate');
                },
                //Dispose validation logic when form is closed
                formClosed: function (event, data) {
                    data.form.validationEngine('hide');
                    data.form.validationEngine('detach');
                }
            });
            
            $('#TableContainer2').jtable({
                title: 'Static: Counterparty Group (View Only)',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'SNAME ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetGroupViewByFilter") %>'                
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    //CHILD TABLE DEFINITION FOR "CTPY"
                    LIMIT: {
                        title: 'No.',
                        width: '3%',
                        sorting: false,
                        create: false,
                        edit: false,
                        display: function (CTPYData) {
                            //Create an image that will be used to open child table
                            var $img = $('<img src="../images/list_metro.png" title="Edit limit configuration" />');
                            //Open child table when user clicks the image
                            $img.click(function () {
                                $('#TableContainer2').jtable('openChildTable',
                                    $img.closest('tr'), //Parent row
                                    {
                                    title: CTPYData.record.SNAME + ' - Limit Setting Group',
                                    actions: {
                                        listAction: '<%= Page.ResolveClientUrl("CounterpartyInfo.aspx/GetCtpyLimitGroupViewByCtpyID?ID=' + CTPYData.record.ID + '") %>'                                       
                                    },
                                    fields: {
                                        SNAME: {
                                            title: 'Short Name',
                                            width: '10%'
                                        },
                                        PCE_ALL: {
                                            title: 'PCE-ALL',
                                            width: '10%'
                                        },
                                        PCE_FI: {
                                            title: 'PCE-FI',
                                            width: '10%'
                                        },
                                        PCE_IRD: {
                                            title: 'PCE-IRD',
                                            width: '10%'
                                        },
                                        PCE_FX: {
                                            title: 'PCE-FX',
                                            width: '10%'
                                        },
                                        PCE_REPO: {
                                            title: 'PCE-REPO',
                                            width: '10%'
                                        },
                                        SET_ALL: {
                                            title: 'SET-ALL',
                                            width: '10%'
                                        }
                                    },
                                    //Initialize validation logic when a form is created
                                    formCreated: function (event, data) {

                                        data.form.find('input[name="AMOUNT"]').number(true);
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                            });
                            //Return image to show on the person row
                            return $img;
                        }
                    },
                    SNAME: {
                        title: 'Short Name',
                        width: '10%'
                    },
                    PCE_ALL: {
                        title: 'PCE-ALL',
                        width: '10%'
                    },
                    PCE_FI: {
                        title: 'PCE-FI',
                        width: '10%'
                    },
                    PCE_IRD: {
                        title: 'PCE-IRD',
                        width: '10%'
                    },
                    PCE_FX: {
                        title: 'PCE-FX',
                        width: '10%'
                    },
                    PCE_REPO: {
                        title: 'PCE-REPO',
                        width: '10%'
                    },
                    SET_ALL: {
                        title: 'SET-ALL',
                        width: '10%'
                    }
                }               
            });


            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    name: $('#name').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton2').click(function (e) {
                e.preventDefault();
                $('#TableContainer2').jtable('load', {
                    name: $('#name2').val()
                });
            });
            $('#name2').keypress(function (event) {
                if (event.which == 13) { //enter
                    event.preventDefault();
                    $('#LoadRecordsButton2').click();
                }
            });
            //Load all records when page is first shown
            $('#LoadRecordsButton2').click();

            $("#select-product").bind('dialogclose', function(event) {
                    $('#select-product').jtable('destroy');
            });
        });
        
    </script>
 </asp:Content>
