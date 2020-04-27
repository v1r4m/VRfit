const MiBand = require('miband');
var usb = require('usb');
const bluetooth = require("webbluetooth").bluetooth;
var app = require('express')();
var server = require('http').createServer(app);
var io = require("socket.io").listen(999);


io.sockets.on("connection", function(socket){
    console.log("socket connected!")

    socket.on("Msg", function(data){
        console.log(data);
        socket.emit("MsgRes","msgfromnode.js");
    });
});

function sendData(d)
{
  io.sockets.emit('hr', d);
}


async function main()
{
    const device = await bluetooth.requestDevice({
        filters: [
          { services: [ MiBand.advertisementService ] }
        ],
        optionalServices: MiBand.optionalServices
      });
       
      const server = await device.gatt.connect();
       
      let miband = new MiBand(server);
      await miband.init();
       
      console.log('Notifications demo...');
      await miband.showNotification('message');
      console.log('Heart Rate Monitor (single-shot)');
      console.log('Result:', await miband.hrmRead());
      for(var i = 0; i<5; i ++)
      {
        await delay(1000);
//        console.log('Result:', await miband.hrmRead());
        sendData(await miband.hrmRead());
      }
}


function delay() {
    return new Promise(resolve => setTimeout(resolve, 300));
}

main();