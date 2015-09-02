<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="UserFunctionMaster.aspx.cs" Inherits="KK.DealMaker.Web.Admin.FunctionMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">

    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label >
                    Function Code:
                    <input type="text" name="name" id="name" class="round input-form-textbox"/></label>
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
                title: 'Master: Function List',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'LABEL ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("UserFunctionMaster.aspx/GetFunctionByFilter") %>',
                    createAction: '<%= Page.ResolveClientUrl("UserFunctionMaster.aspx/Create") %>',
                    updateAction: '<%= Page.ResolveClientUrl("UserFunctionMaster.aspx/Update") %>'
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    USERCODE: {
                        title: 'Function Code',
                        width: '20%',
                        inputClass: 'validate[required]'
                    },
                    LABEL: {
                        title: 'Function Name',
                        width: '50%',
                        inputClass: 'validate[required]'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        width: '12%',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        defaultValue: 'true',
                        formText: 'Active'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
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
                    code: $('#name').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
        });

    </script>
</asp:Content>
