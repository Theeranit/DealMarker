<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="UserProfileFunction.aspx.cs" Inherits="KK.DealMaker.Web.Admin.UserProfileFunction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">

    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label >
                   Function:
                    <select id="function" class="round input-from-dropdownlist"></select></label>
                <label >
                   Profile:
                    <select id="profile" class="round input-from-dropdownlist"></select></label>
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
                title: 'Master: User Profile Function List',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'USER_PROFILE_ID ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/GetProfileFunctionByFilter") %>',
                    createAction: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/Create") %>',
                    updateAction: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/Update") %>',
                    deleteAction: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/Delete") %>'
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    USER_PROFILE_ID: {
                        title: 'Profile Name',
                        width: '15%',
                        options: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/GetProfileOptions") %>',
                        inputClass: 'validate[required]'
                    },
                    FUNCTIONAL_ID: {
                        title: 'Function Name',
                        width: '50%',
                        options: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/GetFunctionOptions") %>',
                        inputClass: 'validate[required]'
                    },
                    ISREADABLE: {
                        title: 'ISREADABLE',
                        width: '10%',
                        type: 'checkbox',
                        values: { 'false': 'Disable', 'true': 'Enable' },
                        //defaultValue: 'true'
                    },
                    ISWRITABLE: {
                        title: 'ISWRITABLE',
                        width: '10%',
                        type: 'checkbox',
                        values: { 'false': 'Disable', 'true': 'Enable' },
                        //defaultValue: 'true'
                    },
                    ISAPPROVABLE: {
                        title: 'ISAPPROVABLE',
                        width: '10%',
                        type: 'checkbox',
                        values: { 'false': 'Disable', 'true': 'Enable' },
                        //defaultValue: 'true'
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
                    strprofile: $('#profile').val(),
                    strfunction:  $('#function').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
           
            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/GetProfileOptions") %>',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#profile").get(0).options.length = 0;
                    $("#profile").get(0).options[0] = new Option("ALL", "-1");

                    $.each(msg.d.Options, function (index, obj) {
                        $("#profile").get(0).options[$("#profile").get(0).options.length] = new Option(obj.DisplayText, obj.Value);
                    });
                },
                error: function () {
                    $("#profile").get(0).options.length = 0;
                    $("#profile").get(0).options[0] = new Option("Please select", "-1");
                }
            });

            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("UserProfileFunction.aspx/GetFunctionOptions") %>',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#function").get(0).options.length = 0;
                    $("#function").get(0).options[0] = new Option("ALL", "-1");

                    $.each(msg.d.Options, function (index, obj) {
                        $("#function").get(0).options[$("#function").get(0).options.length] = new Option(obj.DisplayText, obj.Value);
                    });
                },
                error: function () {
                    $("#function").get(0).options.length = 0;
                    $("#function").get(0).o4ptions[0] = new Option("Please select", "-1");
                }
            });
        });

    </script>
</asp:Content>
