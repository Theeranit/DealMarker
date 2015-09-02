<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="BondInstrumentInfo.aspx.cs" Inherits="KK.DealMaker.Web.Deal.BondInstrumentInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label>
                    Label:
                    <input type="text" name="label" id="label" class="round input-form-textbox"/></label>
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
                title: 'Static: Bond Definition',
                paging: true,
                pageSize: 10,
                sorting: true,
                defaultSorting: 'LABEL ASC',
                columnResizable: true,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("BondInstrumentInfo.aspx/GetByFilter") %>'
                    <% if (Writable) {
                        Response.Write(", createAction: '" + Page.ResolveClientUrl("BondInstrumentInfo.aspx/Create") + "'");
                        Response.Write(", updateAction: '" + Page.ResolveClientUrl("BondInstrumentInfo.aspx/Update") + "'");
                    } %>
                },
                fields: {
                    ID: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    LABEL: {
                        title: 'Label',
                        width: '10%'
                    },
                    PRODUCT_ID: {
                        title: 'Product',
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    INS_MKT: {
                        title: 'Instrument Market',
                        width: '15%',
                        options:'<%= Page.ResolveClientUrl("BondInstrumentInfo.aspx/GetBondMarketOptions") %>',
                        defaultValue: ''
                    },
                    CURRENCY_ID1: {
                        title: 'Currency',
                        width: '10%',
                        options:'<%= Page.ResolveClientUrl("BondInstrumentInfo.aspx/GetCurrencyOptions") %>',
                        defaultValue: ''
                    },
                    ISSUER: {
                        title: 'Issuer',
                        width: '10%'
                    },
                    MATURITY_DATE: {
                        title: 'Maturity Date',
                        width: '10%',
                        type: 'date',
                        displayFormat: 'dd-M-yy'
                    },
                    LOT_SIZE: {
                        title: 'Face Value',
                        width: '10%',
                        list: false,
                        type: 'number'
                    },
                    FLAG_FIXED: {
                        title: 'Fixed',
                        width: '5%',
                        type: 'checkbox',
                        values: { 'false': 'FLOAT', 'true': 'FIXED' },
                        defaultValue: 'true'
                    },
                    COUPON: {
                        title: 'Coupon',
                        width: '10%',
                        type: 'number',
                        displayFormat: '4'
                    },
                    COUPON_FREQ_TYPE_ID: {
                        title: 'Frequency',
                        width: '12%',
                        options: '<%= Page.ResolveClientUrl("BondInstrumentInfo.aspx/GetFrequencyOptions") %>'
                    },
                    CAL_METHOD: {
                        title: 'Method',
                        width: '15%'
                    },
                    ISACTIVE: {
                        title: 'Status',
                        width: '5%',
                        type: 'checkbox',
                        values: { 'false': 'Inactive', 'true': 'Active' },
                        defaultValue: 'true',
                        formText: 'Active'
                    }
                },
                //Initialize validation logic when a form is created
                formCreated: function (event, data) {
                    data.form.find('input[name="LABEL"]').addClass('validate[required]');
                    data.form.find('select[name="INS_MKT"]').addClass('validate[required]');
                    data.form.find('select[name="CURRENCY_ID1"]').addClass('validate[required]');
                    data.form.find('input[name="ISSUER"]').addClass('validate[required]');
                    data.form.find('input[name="MATURITY_DATE"]').addClass('validate[required]');
                    data.form.find('input[name="LOT_SIZE"]').addClass('validate[required]').number(true);
                    data.form.find('input[name="COUPON"]').addClass('validate[required]').number(true, 4);
                    data.form.find('input[name="CAL_METHOD"]').addClass('validate[required]');
                    data.form.find('input[name="LABEL"]').autocomplete({
                        minLength: 4,
                        source: function (request, response) {
                            var jsdata = { name_startsWith: request.term };

                            $.ajax({
                                type: "POST",
                                url: '<%= Page.ResolveClientUrl("BondInstrumentInfo.aspx/GetOPICSInstrumentByLabel") %>',
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(jsdata),
                                success: function (data) {
                                    //alert(data);
                                    response($.map(data.d.Records, function (item) {
                                        return {
                                            label: item.SECID,
                                            issuer: item.ISSUER,
                                            denom: item.DENOM,
                                            rate: item.COUPRATE_8,
                                            mdate: item.MDATE,
                                            method: item.INTCALCRULE,
                                            freq: item.PMTFREQ,
                                            ccy: item.CCY
                                        }
                                    }));
                                }
                            });
                        },
                        select: function (event, ui) {
                            $('input[name="LABEL"]').val(ui.item.label);
                            $('input[name="ISSUER"]').val(ui.item.issuer);
                            $('input[name="LOT_SIZE"]').val(ui.item.denom);
                            $('input[name="COUPON"]').val(ui.item.rate);
                            $('input[name="MATURITY_DATE"]').val(ui.item.mdate);
                            $('input[name="CAL_METHOD"]').val(ui.item.method);
                            $('select[name="CURRENCY_ID1"] option').filter(function() {
                                return $.trim($(this).text()) == $.trim(ui.item.ccy); 
                            }).prop('selected', true);
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
