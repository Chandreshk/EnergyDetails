

var lat;
var lon;

$(document).ready(function(){
	$("div[data-type='plugin']").html("<img id='close' src='../Content/_images/close.png'>");
$("#close").click(function() {
	$("div[data-type='plugin']").hide();
	});
if (navigator.geolocation) {
	 navigator.geolocation.getCurrentPosition(function(position) {
   lat = position.coords.latitude;
   lon= position.coords.longitude;
   var id = $("div[data-type='plugin']");
   set(id);
  });
}
else{
	$("div[data-type='plugin']").html("Oops! no location found");
}
});
function set(ht) {
    this.preventDefault();
 	$.ajax({
            type: "GET",
            url: "http://api.openweathermap.org/data/2.5/weather?lat="+lat+"&lon="+lon+"&APPID=9a0bfebddb482e7b0dd96255f3b22f67",
            datatype: "json",
            async: true,
            success:function(data){
            	var date= new Date(data.dt*1000);
            	$(ht).append("<h5 id='city' class='text'>"+data.name+"</h5>");
            	$(ht).append("<img id='icon' src='../Content/_images/"+data.weather[0].icon+".png' width='30' height='30'>");
            	$(ht).css("background","url(../Content/_images/"+data.weather[0].icon+".jpg)");
            	var num =data.main.temp-272.15;
					var n = parseInt(num, 10);
            	$(ht).append("<h5 id='temp' class='text'>"+n+"&deg;C</h5>");
            	$(ht).append("<h5 id='desc' class='text'>"+data.weather[0].description+"</h5>");
            	
            }
         });
        }
