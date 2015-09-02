<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="InstrumentsMaster.aspx.cs" Inherits="KK.DealMaker.Web.Admin.InstrumentsMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                    <label>Label:<input type="text" name="label" id="label" class="round input-form-textbox"/></label>
                    <label>Product:<select id="product" class="round input-from-dropdownlist"></select></label>
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
            
            set_options('product', 'ALL', 'GetProductNoBondOptions');

            //Prepare jtable plugin
            $('#TableContainer').jtable({
                title: 'Static: Instruments',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'LABEL ASC',
                columnResizable: true,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("InstrumentsMaster.aspx/GetByFilter") %>',
                    createAction: '<%= Page.ResolveClientUrl("InstrumentsMaster.aspx/Create") %>',
                    updateAction: '<%= Page.ResolveClientUrl("InstrumentsMaster.aspx/Update") %>'
                },
                fields: {
                    ID: {
                        title: 'ID',
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    PRODUCT_ID: {
                        title: 'Product',
                        options: '<%= Page.ResolveClientUrl("InstrumentsMaster.aspx/GetProductNoBondOptions") %>'
                    },
                    LABEL: {
                        title: 'Label',
                    },
                    CURRENCY_ID1: {
                        title: 'CCY1',
                        options: '<%= Page.ResolveClientUrl("InstrumentsMaster.aspx/GetCurrencyOptions") %>'
                    },
                    CURRENCY_ID2: {
                        title: 'CCY2',
                        options: '<%= Page.ResolveClientUrl("InstrumentsMaster.aspx/GetCurrencyOptions") %>'
                    },
                    FLAG_MULTIPLY: {
                        title: 'Flag multiply',
                        type: 'checkbox',
                        values: { 'false': 'FALSE', 'true': 'TRUE' },
                        defaultValue: 'true'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        defaultValue: 'true',
                        formText: 'Active'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="LABEL"]').addClass('validate[required]');
                    data.form.find('select[name="PRODUCT_ID"]').addClass('validate[required]');

                    if (data.form.find('select[name="PRODUCT_ID"] option:selected').text().toUpperCase().indexOf('FX') < 0) {
                        data.form.find('select[name="CURRENCY_ID1"]').prop('disabled', true);
                        data.form.find('select[name="CURRENCY_ID2"]').prop('disabled', true);
                        data.form.find('input[name="FLAG_MULTIPLY"]').prop('disabled', true);
                    }
                    else {
                        $('select[name="CURRENCY_ID1"]').addClass('validate[required]');
                        $('select[name="CURRENCY_ID2"]').addClass('validate[required]');
                    }                    

                    $('select[name="PRODUCT_ID"]').on('change', function (event) {
                        if ($('select[name="PRODUCT_ID"] option:selected').text().toUpperCase().indexOf('FX') < 0) {
                            $('select[name="CURRENCY_ID1"]').prop('disabled', true);
                            $('select[name="CURRENCY_ID2"]').prop('disabled', true);
                            $('input[name="FLAG_MULTIPLY"]').prop('disabled', true);

                            $('select[name="CURRENCY_ID1"]').removeClass('validate[required]');
                            $('select[name="CURRENCY_ID2"]').removeClass('validate[required]');
                        }
                        else {
                            $('select[name="CURRENCY_ID1"]').prop('disabled', false);
                            $('select[name="CURRENCY_ID2"]').prop('disabled', false);
                            $('input[name="FLAG_MULTIPLY"]').prop('disabled', false);

                            $('select[name="CURRENCY_ID1"]').addClass('validate[required]');
                            $('select[name="CURRENCY_ID2"]').addClass('validate[required]');
                        }
                        data.form.validationEngine('hide');
                        data.form.validationEngine('validate');
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
                    label: $('#label').val(),
                    product: $('#product').val() == null ? -1 : $('#product').val()
                });
            });

            //Load all records when page is first shown
            $('#LoadRecordsButton').click();

        });

    </script>
</asp:Content>
