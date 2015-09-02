<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="CSATypeMaster.aspx.cs" Inherits="KK.DealMaker.Web.Admin.CSATypeMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Label:<input type="text" name="label" id="label" class="round input-form-textbox"/></label>
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
                title: 'CSA Type',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'LABEL ASC',
                columnResizable: true,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("CSATypeMaster.aspx/GetByFilter") %>',
                    createAction: '<%= Page.ResolveClientUrl("CSATypeMaster.aspx/Create") %>',
                    updateAction: '<%= Page.ResolveClientUrl("CSATypeMaster.aspx/Update") %>'
                },
                fields: {
                    ID: {
                        title: 'ID',
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    
                    LABEL: {
                        title: 'Type'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="LABEL"]').addClass('validate[required]');

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
