<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="TempLimitInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.TempLimitInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%= Page.ResolveClientUrl("~/Scripts/autoNumeric.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <form id="Form1" runat="server">
        <div class="content-module-main">
            <div class="site-container">
                <div class="filtering" id="dvFiltering">
                        <label>Counterparty:<input type="text" id="ctpy" class="round input-form-textbox"/></select></label>
                        <label>Limit:<input type="text" id="limit" class="round input-form-textbox"/></label>
                        <label>Effective Date: <input type="text" name="effdatefrom" id="effdatefrom" class="round input-form-textbox"/></label>
                        <label>to:<input type="text" name="effdateto" id="effdateto" class="round input-form-textbox"/></label>
                        <label>Expiry Date: <input type="text" name="expdatefrom" id="expdatefrom" class="round input-form-textbox"/></label>
                        <label>to:<input type="text" name="expdateto" id="expdateto" class="round input-form-textbox"/></label>
                        <button type="submit" id="LoadRecordsButton" class="round blue button-submit">Load records</button>
                </div>
            </div>
        </div>
        <div class="table-container">
            <div id="TableContainer">
            </div>
        </div>
        <div class="content-module-main">
            <div class="site-container">
                <div class="filtering" id="dvFilter2">
                    <label>Country:<input type="text" name="country" id="country" class="round input-form-textbox" /></label>
                    <label>Effective Date: <input type="text" name="cteffdatefrom" id="cteffdatefrom" class="round input-form-textbox"/></label>
                    <label>to:<input type="text" name="cteffdateto" id="cteffdateto" class="round input-form-textbox"/></label>
                    <label>Expiry Date: <input type="text" name="ctexpdatefrom" id="ctexpdatefrom" class="round input-form-textbox"/></label>
                    <label>to:<input type="text" name="ctexpdateto" id="ctexpdateto" class="round input-form-textbox"/></label>
                    <button type="submit" id="LoadRecordsButton2" class="round blue button-submit">Load records</button>
                </div>
            </div>
        </div>  
    </form>
     <div class="table-container">
         <div id="TableContainer2">
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#effdatefrom,#effdateto,#expdatefrom,#expdateto').datepicker({ dateFormat: "dd/mm/yy", changeYear: true });
            $('#cteffdatefrom,#cteffdateto,#ctexpdatefrom,#ctexpdateto').datepicker({ dateFormat: "dd/mm/yy", changeYear: true });

            //Temp Counterparty Limit Table
            $('#TableContainer').jtable({
                title: 'Static: Temp Counterparty Limit',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'CTPY_LIMIT_ID ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("TempLimitInfo.aspx/GetByFilter") %>'
                    <% if (Writable) {
                        Response.Write(", createAction: '" + Page.ResolveClientUrl("TempLimitInfo.aspx/Create") + "'");
                        Response.Write(", updateAction: '" + Page.ResolveClientUrl("TempLimitInfo.aspx/Update") + "'");
                    } %>
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },                    
                    CTPY_LIMIT_ID: {
                        title: 'Counterparty-Limit'
                        , options: '<%= Page.ResolveClientUrl("TempLimitInfo.aspx/GetCtpyLimitOptions") %>'
                    },
                    AMOUNT: {
                        title: 'Amount'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    EFFECTIVE_DATE: {
                        title: 'Effective Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    EXPIRY_DATE: {
                        title: 'Expiry Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        formText: 'Active',
                        defaultValue: 'true'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="CTPY_LIMIT_ID"]').addClass('validate[required]');
                    data.form.find('input[name="EFFECTIVE_DATE"]').addClass('validate[required]');
                    data.form.find('input[name="EXPIRY_DATE"]').addClass('validate[required]');
                    data.form.find('input[name="AMOUNT"]').addClass('validate[required]');
                    data.form.find('input[name="AMOUNT"]').number(true);
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

            //Temp Country Limit Table
            $('#TableContainer2').jtable({
                title: 'Static: Temp Country Limit',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'COUNTRY_ID ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("TempLimitInfo.aspx/GetTempCountryByFilter") %>'
                    <% if (Writable) {
                        Response.Write(", createAction: '" + Page.ResolveClientUrl("TempLimitInfo.aspx/CreateTempCountry") + "'");
                        Response.Write(", updateAction: '" + Page.ResolveClientUrl("TempLimitInfo.aspx/UpdateTempCountry") + "'");
                    } %>
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },                    
                    COUNTRY_ID: {
                        title: 'Country'
                        , options: '<%= Page.ResolveClientUrl("TempLimitInfo.aspx/GetCountryOptions") %>'
                    },
                    AMOUNT: {
                        title: 'Amount'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    EFFECTIVE_DATE: {
                        title: 'Effective Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    EXPIRY_DATE: {
                        title: 'Expiry Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        formText: 'Active',
                        defaultValue: 'true'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="COUNTRY_ID"]').addClass('validate[required]');
                    data.form.find('input[name="EFFECTIVE_DATE"]').addClass('validate[required]');
                    data.form.find('input[name="EXPIRY_DATE"]').addClass('validate[required]');
                    data.form.find('input[name="AMOUNT"]').addClass('validate[required]');
                    data.form.find('input[name="AMOUNT"]').number(true);
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
                    strCtpy: $('#ctpy').val(),
                    strLimit: $('#limit').val(),
                    strEffDateFrom: $('#effdatefrom').val(),
                    strEffDateTo: $('#effdateto').val(),
                    strExpDateFrom: $('#expdatefrom').val(),
                    strExpDateTo: $('#expdateto').val()
                });
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton2').click(function (e) {
                e.preventDefault();
                $('#TableContainer2').jtable('load', {
                    strCountry: $('#country').val(),
                    strEffDateFrom: $('#cteffdatefrom').val(),
                    strEffDateTo: $('#cteffdateto').val(),
                    strExpDateFrom: $('#ctexpdatefrom').val(),
                    strExpDateTo: $('#ctexpdateto').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
            $('#LoadRecordsButton2').click();

            $('#country').keypress(function (event) {
                if (event.which == 13) { //enter
                    event.preventDefault();
                    $('#LoadRecordsButton2').click();
                }
            });
        });

    </script>
</asp:Content>
