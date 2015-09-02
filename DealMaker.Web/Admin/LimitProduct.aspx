<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="LimitProduct.aspx.cs" Inherits="KK.DealMaker.Web.Admin.LimitProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">

    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label >
                   Product:
                    <select id="product" class="round input-from-dropdownlist"></select></label>
                <label >
                   Liimt:
                    <select id="limit" class="round input-from-dropdownlist"></select></label>
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
                title: 'Master: Limit Product List',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'ID ASC',
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("LimitProduct.aspx/GetLimitProductByFilter") %>',
                    createAction: '<%= Page.ResolveClientUrl("LimitProduct.aspx/Create") %>',
                    updateAction: '<%= Page.ResolveClientUrl("LimitProduct.aspx/Update") %>'
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    LIMIT_ID: {
                        title: 'Limit Name',
                        width: '20%',
                        options: '<%= Page.ResolveClientUrl("LimitProduct.aspx/GetLimitOptions") %>',
                        inputClass: 'validate[required]'
                    },
                    PRODUCT_ID: {
                        title: 'Product Name',
                        width: '20%',
                        options: '<%= Page.ResolveClientUrl("LimitProduct.aspx/GetProductOptions") %>',
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

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#TableContainer').jtable('load', {
                    strproduct: $('#product').val(),
                    strlimit:  $('#limit').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();
           
            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("LimitProduct.aspx/GetProductOptions") %>',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#product").get(0).options.length = 0;
                    $("#product").get(0).options[0] = new Option("ALL", "-1");

                    $.each(msg.d.Options, function (index, obj) {
                        $("#product").get(0).options[$("#product").get(0).options.length] = new Option(obj.DisplayText, obj.Value);
                    });
                },
                error: function () {
                    $("#product").get(0).options.length = 0;
                    $("#product").get(0).options[0] = new Option("Please select", "-1");
                }
            });

            $.ajax({
                type: "POST",
                url: '<%= Page.ResolveClientUrl("LimitProduct.aspx/GetLimitOptions") %>',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#limit").get(0).options.length = 0;
                    $("#limit").get(0).options[0] = new Option("ALL", "-1");

                    $.each(msg.d.Options, function (index, obj) {
                        $("#limit").get(0).options[$("#limit").get(0).options.length] = new Option(obj.DisplayText, obj.Value);
                    });
                },
                error: function () {
                    $("#limit").get(0).options.length = 0;
                    $("#limit").get(0).o4ptions[0] = new Option("Please select", "-1");
                }
            });
        });

    </script>
</asp:Content>
