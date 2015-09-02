<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="KK.DealMaker.Web.Admin.UserMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    
    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label>
                    Name:
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
                title: 'Master: User List',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'NAME ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("UserMaster.aspx/GetByFilter") %>',
                    createAction: '<%= Page.ResolveClientUrl("UserMaster.aspx/CreateUser") %>',
                    updateAction: '<%= Page.ResolveClientUrl("UserMaster.aspx/UpdateUser") %>'
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    NAME: {
                        title: 'Name',
                        width: '25%'
                    },
                    USERCODE: {
                        title: 'Usercode',
                        width: '15%'
                    },
                    DEPARTMENT: {
                        title: 'Department',
                        width: '10%'
                    },
                    USER_OPICS: {
                        title: 'User Opics',
                        width: '10%'
                    },
                    USER_PROFILE_ID: {
                        title: 'Profile',
                        width: '12%',
                        options: '<%= Page.ResolveClientUrl("UserMaster.aspx/GetProfileOptions") %>'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        width: '12%',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        defaultValue: 'true',
                        formText: 'Active'
                    },
                    ISLOCKED: {
                        title: 'Locked',
                        width: '12%',
                        type: 'checkbox',
                        values: { 'false': 'Active', 'true': 'Locked' },
                        defaultValue: 'false'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="NAME"]').addClass('validate[required]');
                    data.form.find('input[name="USERCODE"]').addClass('validate[required]');
                    data.form.find('input[name="DEPARTMENT"]').addClass('validate[required]');
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
                    name: $('#name').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
        });

    </script>
</asp:Content>
