<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="CountryInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.CountryInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Short name:<input type="text" name="label" id="label" class="round input-form-textbox"/></label>
                    <button type="submit" id="LoadRecordsButton" class="round blue button-submit">
                    Load records</button>
                </form>
            </div>
        </div>
    </div>
    <div class="table-container">
        <div id="TableContainer">
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            
            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Static: Country',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'LABEL ASC',
                columnResizable: true,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("CountryInfo.aspx/GetByFilter") %>'
                    <% if (Writable) {
                        Response.Write(", createAction: '" + Page.ResolveClientUrl("CountryInfo.aspx/Create") + "'");
                        Response.Write(", updateAction: '" + Page.ResolveClientUrl("CountryInfo.aspx/Update") + "'");
                    } %>
                },
                fields: {
                    ID: {
                        title: 'ID',
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
                        display: function (CountryData) {
                            //Create an image that will be used to open child table
                            var $img = $('<img src="../images/list_metro.png" title="Edit limit configuration" />');
                            //Open child table when user clicks the image
                            $img.click(function () {
                                $('#TableContainer').jtable('openChildTable',
                                    $img.closest('tr'), //Parent row
                                    {
                                    title: CountryData.record.LABEL + ' - Limit',
                                    actions: {
                                        listAction: '<%= Page.ResolveClientUrl("CountryInfo.aspx/GetCountryLimitByCountryID?ID=' + CountryData.record.ID + '") %>'
                                        <% if (Writable) {
                                            Response.Write(", updateAction: '" + Page.ResolveClientUrl("CountryInfo.aspx/UpdateCountryLimit") + "'");
                                        } %>
                                    },
                                    fields: {
                                        COUNTRY_ID: {
                                            type: 'hidden',
                                            defaultValue: CountryData.record.ID
                                        },
                                        ID: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        AMOUNT: {
                                            title: 'Amount',
                                            type: 'number',
                                            inputClass: 'validate[required]',
                                            listClass: 'right'
                                        },
                                        EXPIRY_DATE: {
                                            title: 'Expire date',
                                            type: 'date',
                                            displayFormat: 'dd-M-yy',
                                            inputClass: 'validate[required]'
                                        },
                                        FLAG_CONTROL: {
                                            title: 'Control',
                                            type: 'checkbox',
                                            values: { 'false': 'No-Restriction', 'true': 'Control' },
                                            defaultValue: 'true',
                                            formText: 'Control'
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
                    LABEL: {
                        title: 'Short Name',
                        width: '48%'
                    },
                    DESCRIPTION: {
                        title: 'Description',
                        width: '49%'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="LABEL"]').addClass('validate[required]');
                    data.form.find('input[name="DESCRIPTION"]').addClass('validate[required]');
                    data.form.find('input[name="LABEL"]').prop('maxlength','2');
                    data.form.find('input[name="LABEL"]').autocomplete({
                        minLength: 1,
                        source: function (request, response) {
                            var jsdata = { label_startsWith: request.term };

                            $.ajax({
                                type: "POST",
                                url: '<%= Page.ResolveClientUrl("CountryInfo.aspx/GetOPICSCountryByLabel") %>',
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(jsdata),
                                success: function (data) {
                                    //alert(data);
                                    response($.map(data.d.Records, function (item) {
                                        return {
                                            label: item.CCODE,
                                            desc: item.COUN,
                                        }
                                    }));
                                }
                            });
                        },
                        select: function (event, ui) {
                            //alert(ui.item.label);
                            $('input[name="LABEL"]').val(ui.item.label);
                            $('input[name="DESCRIPTION"]').val(ui.item.desc);
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

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    label: $('#label').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();

        });

    </script>
</asp:Content>
