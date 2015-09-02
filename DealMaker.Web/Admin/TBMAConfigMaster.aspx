<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="TBMAConfigMaster.aspx.cs" Inherits="KK.DealMaker.Web.Admin.TBMAConfigMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    <%--<div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label>Status Name:<input type="text" name="name" id="name" class="round input-form-textbox"/></label>
                    <button type="submit" id="LoadRecordsButton" class="round blue button-submit">
                    Load records</button>
                </form>
            </div>
        </div>
    </div>--%>
    <div class="table-container">
        <div id="TableContainer">
        </div>
    </div>        
    <script type="text/javascript">

        $(document).ready(function () {

            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Master: TBMA Config',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("TBMAConfigMaster.aspx/GetAll") %>',
                    updateAction: '<%= Page.ResolveClientUrl("TBMAConfigMaster.aspx/Update") %>'
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    TBMA_CAL_USERNAME: {
                        title: 'CAL_USERNAME',
                        inputClass: 'validate[required]'
                    },
                    TBMA_CAL_PASSWORD: {
                        title: 'CAL_PASSWORD',
                        inputClass: 'validate[required]'
                    },
                    TBMA_RPT_PATH: {
                        title: 'RPT_PATH',
                        inputClass: 'validate[required]'
                    },
                    TBMA_RPT_PREFIX: {
                        title: 'RPT_PREFIX',
                        inputClass: 'validate[required]'
                    },
                    TBMA_RPT_TRADERID: {
                        title: 'RPT_TRADERID',
                        inputClass: 'validate[required]'
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
               
            //Load all records when page is first shown
            $('#TableContainer').jtable('load');
        });

    </script>

</asp:Content>
