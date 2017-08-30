<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportShedual.aspx.cs" Inherits="EnergyDetails.ReportShedual" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script src="~/Content/js/jquery-v1.6.1.js" type="text/javascript"></script>
<script src="~/Content/js/jquery-1.12.4.js"></script>
<script src="~/Content/js/jquery-ui.js"></script>
<script src="~/Content/js/highcharts.js"></script>
<script src="~/Content/js/highcharts-more.js"></script>
<script src="~/Content/js/data.js"></script>
<script src="~/Content/js/exporting.js"></script>
<script src="~/Content/js/fusioncharts.js"></script>
<script src="~/Content/js/fusioncharts.widgets.js"></script>
<script src="~/Content/js/fusioncharts.theme.fint.js"></script>
<script src="~/Content/js/translate.js"></script>
<link href="~/Content/css/common.css" rel="stylesheet" />
<script src="~/Content/js/app.js"></script>
<script src="~/Content/js/autocomplete.js"></script>
<link href="~/Content/css/weather.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.2/jspdf.min.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <div id="ExportPdf">
                <div style="margin-left:0%;margin-top:0%; ">
                    <div id="container"></div>
                </div>  
                <div id="totalbuild" style="width:1200px">
                    <fieldset style="border:2px solid #031439;">
                        <legend style="color:#031439"><b style="font-size: 23px;">Summary</b></legend>
                        <div id="tblSpedometer" style="text-align:-webkit-center;overflow-x:auto;max-width:90%;min-width:40%;align:center">
                        </div>
                    </fieldset>
                </div>
            </div>
    </div>
    </form>
</body>
    <script>

        //function SearchDatainasset(value) {
        //    if (FlagWether == true) {
        //        $("#hdndlconsuption").val('Weather');
        //    }
        //    FlagWether = false;
        //    var Monthnumber = value;
        //    var LocationID = $("#hdnLocationId").val();
        //    var LocatioName = $("#hdnlocatiname").val();
        //    var BuilDID = value;
        //    var DailyConsuption = $("#hdndlconsuption").val();
        //    if (BuilDID == 'YearlyEnergy' || BuilDID == 'YearlyCost' || BuilDID == 'monthlyIntensity' || BuilDID == 'YearlyIntensity') {
        //        Monthnumber = $("#hdndlconsuption").val();
        //    }
        //    var BuildinName = "";
        //    if (FlagBuild == true && value != undefined) {
        //        var ReportType = $("#hdndlconsuption").val();
        //        BuildinName = ': ' + $("#hdnBuildinName").val();
        //        var LocatioName = LocatioName;
        //        Monthnumber = ReportType;
        //        FlagBuild = false;
        //        FlagWether = false;
        //    }
        //    else if (value == 'Daily') {
        //        Monthnumber = "DailyColumn";
        //        BuilDID = "";
        //    }
        //    else if (value == 'Cost') {
        //        Monthnumber = $("#hdndlconsuption").val();
        //    }
        //    var UrlA = "";
        //    if (value != undefined) {
        //        UrlA = '../EnergyLevel/GetHighChart';
        //    }
        //    else {
        //        return false;
        //    }
        //    $("#trspedometer").css("display", "");
        //    $("#txtlegen").css("display", "");
        //    var url = UrlA;
        //    var data1 = { Monthnumber: Monthnumber, BuilDID: BuilDID, LocationID: LocationID };
        //    var dsnew = "";
        //    $.get(url, data1, function (data1) {
        //        var JString = data1;
        //        if (JString == "Fail") {
        //            return false;
        //        }
        //        else {
        //            dsnew = JSON.parse(JString);
        //        }
        //        if (dsnew.Table.length > 0) {
        //            $("#tblSpedometer").html("");
        //            var OriginalAmount = [];
        //            var ArrData1 = [];
        //            var AssetNumberDName = [], AssetNamename = [], ReceivedDatenName = [], AssetName4 = [], AssetData4 = [];
        //            var DateMonth = [];
        //            var AssetIDD = [];
        //            var AssetIDDName = [];
        //            var NewSumAsseteName = "";
        //            var AssetNumberD = [], AssetName = [], ReceivedDate = [], NewSumAssete = [];

        //            if (Monthnumber != 'EnergyYearly' && Monthnumber != 'EnergyYearlyIntensity') {
        //                if (dsnew.Table.length > 0) {
        //                    $("#totalbuild").css("display", "block");
        //                    $("#tblSpedometer").html("");
        //                    var Spedometer = "";
        //                    Spedometer = "<table><tr>";
        //                    for (var a = 0; a < dsnew.Table1.length; a++) {
        //                        {
        //                            Spedometer += "<td id='Spedometer" + a + "'></td>";
        //                        }
        //                    }
        //                    Spedometer += "</tr></table>";
        //                    $("#tblSpedometer").html(Spedometer);
        //                }
        //            }
        //            else {
        //                $("#totalbuild").css("display", "none");
        //            }

        //            var AssetData = [];
        //            var AssetName = "";
        //            var SerisData = [];
        //            var ArrayData = [];
        //            var ArraDataconvert = [];
        //            // if (BuilDID == 1) {
        //            var tableonelenths = "";
        //            var BuildingID = "";
        //            SerisData += '[';
        //            if (Monthnumber == "Energy") {
        //                tableonelenths = dsnew.Table1.length;
        //            }
        //            else {
        //                tableonelenths = dsnew.Table1.length;
        //            }
        //            if (tableonelenths > 0) {
        //                BuildingID = '<div>';
        //                for (var i = 0; i < tableonelenths; i++) {
        //                    var ArrayDate = "";
        //                    if (dsnew.Table1[i].AssetID != 0) {
        //                        AssetData = [];
        //                        for (var j = 0; j < dsnew.Table.length; j++) {
        //                            if (dsnew.Table[j].AssetID == dsnew.Table1[i].AssetID) {
        //                                if (Monthnumber == "EnergyYearlyIntensity") {
        //                                    $("#hdnPloatedLine").val(dsnew.Table[j].TargetLine);
        //                                }
        //                                if (dsnew.Table[j].Day == '0') {
        //                                    dsnew.Table[j].Day = '1';
        //                                }
        //                                if (Monthnumber != 'EnergyYearly' && dsnew.Table[j].DateMonth != '0') {
        //                                    dsnew.Table[j].DateMonth = dsnew.Table[j].DateMonth - 1;
        //                                }
        //                                else {
        //                                    dsnew.Table[j].DateMonth = dsnew.Table[j].DateMonth;
        //                                }
        //                                AssetName = (dsnew.Table1[i].AssetName);
        //                                ArrayDate += '[' + [Date.UTC(dsnew.Table[j].GetYear, (dsnew.Table[j].DateMonth), dsnew.Table[j].Day), dsnew.Table[j].Value] + ']';
        //                                ArrayDate += ',';
        //                            }
        //                        }
        //                        BuildingID += '<input type="hidden" value="' + dsnew.Table1[i].AssetID + '" id="' + AssetName.replace(' ', '') + '"/><input type="hidden" value="' + dsnew.Table1[i].AssetID + '" id="' + AssetName.replace(' ', '') + '"/>';
        //                        SerisData += '{name:"' + AssetName + '",data:[' + ArrayDate + ']},';
        //                    }
        //                }
        //                BuildingID += '</div>';
        //            }
        //            $("#spanbuildid").html(BuildingID);

        //            if (Monthnumber != 'Energy' && Monthnumber != 'EnergyYearly' && Monthnumber != 'EnergyYearlyIntensity' && Monthnumber != 'DailyColumn' && Monthnumber != 'Weather') {
        //                var ArrayDateSum = "";
        //                if (dsnew.Table3.length > 0) {
        //                    for (var k = 0; k < dsnew.Table3.length; k++) {
        //                        if (dsnew.Table3[k].AssetID == 0) {
        //                            if (dsnew.Table3[k].Day == '0') {
        //                                dsnew.Table3[k].Day = '1';
        //                            }
        //                            NewSumAsseteName = dsnew.Table3[k].AssetName;
        //                            ArrayDateSum += '[' + [Date.UTC(dsnew.Table3[k].GetYear, (dsnew.Table3[k].DateMonth - 1), dsnew.Table3[k].Day), dsnew.Table3[k].value] + ']';
        //                            ArrayDateSum += ',';
        //                        }
        //                    }
        //                    SerisData += '{name:"' + NewSumAsseteName + '",data:[' + ArrayDateSum + ']},';
        //                }
        //            }
        //            SerisData += ']';
        //            var SerisDataseri = eval(SerisData);
        //            var Type = '';
        //            var title = "";
        //            var subtitle = "";
        //            var YAxisTitle = "";
        //            var FacilityName = $("#hdnfacilityname").val();
        //            var SiteName = $("#hdnsitename").val();
        //            subtitle = LocatioName
        //            title = FacilityName + ' : ' + SiteName + BuildinName;
        //            if (Monthnumber == 'Energy') {
        //                Type = 'column';
        //                SetDailyGraph = 24 * 3600 * 1000;
        //            }
        //            else if (Monthnumber == 'EnergyYearly') {
        //                Type = 'column';
        //                SetDailyGraph = 24 * 3600 * 1000;
        //            }
        //            else if (Monthnumber == 'EnergyYearlyIntensity') {
        //                Type = 'column';
        //                SetDailyGraph = 24 * 3600 * 1000;
        //            }
        //            else if (Monthnumber == 'DailyColumn' || Monthnumber == 'Weather') {
        //                Type = 'column';
        //                SetDailyGraph = 24 * 3600 * 1000;
        //            }

        //            else {
        //                Type = 'line';
        //                SetDailyGraph = 24 * 3600 * 1000;
        //            }
        //            if (subtitle == '@ViewBag.MonthlyTrend' && Monthnumber == 'Energy') {
        //                Type = 'line';
        //                SetDailyGraph = 24 * 3600 * 1000;
        //            }
        //            if (BuilDID == "Cost") {
        //                YAxisTitle = "USD( $ )";
        //            }
        //            else
        //                if (BuilDID == "YearlyCost") {
        //                    YAxisTitle = "USD( $ 0000)";
        //                }
        //                else
        //                    if (Monthnumber == "EnergyYearlyIntensity") {
        //                        YAxisTitle = "kwh/sq. ft.";
        //                    }
        //                    else if (DailyConsuption == 'Daily') {
        //                        YAxisTitle = "kWh";
        //                    }
        //                    else {
        //                        YAxisTitle = "kWh";
        //                    }
        //            var SetDailyGraph = "";
        //            var widthset = '1200';
        //            if (dsnew.Table2[0].Monthly == 'Daily') {
        //                SetDailyGraph = 24 * 3600 * 1000;
        //            }
        //            var PloatedLine = "";
        //            var tgline = "";
        //            if (Monthnumber == 'EnergyYearlyIntensity') {
        //                PloatedLine = $("#hdnPloatedLine").val();
        //                tgline = 'Target Intensity';
        //            }

        //            $('#container').highcharts({
        //                chart: {
        //                    type: Type,
        //                    width: widthset,
        //                },
        //                navigator: {
        //                    enabled: false
        //                },
        //                scrollbar: {
        //                    enabled: true
        //                },
        //                title: {
        //                    text: title,
        //                    style: {
        //                        color: '#333',
        //                        fontWeight: 'bold',
        //                        fontSize: '20px',
        //                        fontFamily: 'Trebuchet MS, Verdana, sans-serif'

        //                    }
        //                },
        //                subtitle: {
        //                    text: subtitle,
        //                    //style: {
        //                    //    fontSize: '18px'
        //                    //}
        //                    style: {
        //                        color: '#333',
        //                        fontWeight: 'bold',
        //                        fontSize: '20px',
        //                        fontFamily: 'Trebuchet MS, Verdana, sans-serif'

        //                    }
        //                },

        //                yAxis: {
        //                    labels: {
        //                        formatter: function () {
        //                            return this.value;
        //                        }
        //                    },
        //                    title: {
        //                        text: YAxisTitle
        //                    },
        //                    x: -20,
        //                    plotBands: [{
        //                        from: PloatedLine,
        //                        to: PloatedLine,
        //                        color: "Black",
        //                        label: {

        //                            "text": tgline,
        //                            y: -5
        //                        }
        //                    }
        //                    ]
        //                },
        //                plotOptions: {
        //                    column: {
        //                        pointPadding: 0,
        //                        borderWidth: 0
        //                    },
        //                    series: {
        //                        events: {
        //                            legendItemClick: function () {
        //                                FlagBuild = true;
        //                                var AssetID = $("#" + this.name).val();
        //                                var BuildId = $("#" + this.name.replace(' ', '')).val();
        //                                if (AssetID != undefined) {
        //                                    //$('#upload').click(function () { // The $ is not necessary - you already have it
        //                                    //    $('#uploadme').click();
        //                                    //});
        //                                    BuildId = "Test";
        //                                    $('input[type=file]').trigger('click');
        //                                }
        //                                $("#hdnBuildinName").val(this.name);
        //                                SearchDatainasset(BuildId)
        //                            }
        //                        }
        //                    }
        //                },
        //                xAxis: {
        //                    type: 'datetime',
        //                    tickInterval: SetDailyGraph,
        //                    dateTimeLabelFormats: {
        //                        day: '%d %b %y'
        //                    },

        //                },

        //                series: SerisDataseri
        //            });
        //            var series = [],
        //               j = 0;
        //            if (Monthnumber != 'EnergyYearly') {
        //                for (j = 0; j < dsnew.Table1.length; j++) {
        //                    Spedometer += "<td id='Spedometer" + j + "'></td>";
        //                    $('#Spedometer' + j).highcharts({
        //                        chart: {
        //                            type: 'gauge',
        //                            plotBackgroundColor: null,
        //                            plotBackgroundImage: null,
        //                            plotBorderWidth: 0,
        //                            plotShadow: false,
        //                            backgroundColor: 'deepskyblue',
        //                            width: 240,
        //                            height: 230
        //                        },
        //                        title: {

        //                            text: dsnew.Table1[j].AssetName,
        //                            style: {
        //                                color: 'black',
        //                                font: 'bold 18px "Trebuchet MS", Verdana, sans-serif'
        //                            }
        //                        },
        //                        pane: {
        //                            startAngle: -150,
        //                            endAngle: 150,
        //                            size: '100%',
        //                            background: [{
        //                                backgroundColor: {
        //                                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
        //                                    stops: [
        //                                        [0, '#FFF'],
        //                                        [1, '#333']
        //                                    ]
        //                                },
        //                                borderWidth: 0,
        //                                outerRadius: '109%'
        //                            }, {
        //                                backgroundColor: {
        //                                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
        //                                    stops: [
        //                                        [0, '#333'],
        //                                        [1, '#FFF']
        //                                    ]
        //                                },
        //                                borderWidth: 1,
        //                                outerRadius: '107%'
        //                            }, {
        //                                // default background
        //                            }, {
        //                                backgroundColor: '#d6d6c2',
        //                                borderWidth: 0,
        //                                outerRadius: '105%',
        //                                innerRadius: '110%'
        //                            }]
        //                        },
        //                        yAxis: {
        //                            min: 0,
        //                            max: dsnew.Table2[0].MaxValue,
        //                            minorTickInterval: 'auto',
        //                            minorTickWidth: 1,
        //                            minorTickLength: 10,
        //                            minorTickPosition: 'inside',
        //                            minorTickColor: '#1f1f7a',
        //                            tickPixelInterval: 30,
        //                            tickWidth: 2,
        //                            tickPosition: 'inside',
        //                            tickLength: 10,
        //                            tickColor: 'blue',
        //                            labels: {
        //                                step: 2,
        //                                rotation: 'auto'
        //                            },
        //                            title: {
        //                                text: YAxisTitle,
        //                                style: {
        //                                    color: 'black'
        //                                }
        //                            },
        //                            plotBands: [{
        //                                //from: 0,
        //                                //to: 120,
        //                                //color: '#55BF3B' // green
        //                            },
        //                            {
        //                                from: 0,
        //                                to: 120,
        //                                innerRadius: 35,
        //                                outerRadius: 10,
        //                                shape: 'circle',
        //                                color: '#c2c2a3' //
        //                            }
        //                            , {
        //                                //from: 120,
        //                                //to: 160,
        //                                //color: '#DDDF0D' // yellow
        //                            }, {
        //                                //from: 160,
        //                                //to: 200,
        //                                //color: '#DF5353' // red
        //                            }]
        //                        },

        //                        series: [{
        //                            name: 'Energy',
        //                            data: [dsnew.Table1[j].value],
        //                            tooltip: {
        //                                //valueSuffix: 'Value'
        //                            }
        //                        }]

        //                    });
        //                }
        //            }
        //        }
        //        else {
        //            return false;
        //        }
        //    });
        //}
    </script>
</html>
