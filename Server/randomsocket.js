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

async function main()
{
    for(var i = 0; i<5000; i ++)
    {
      await delay(1000);
      sendData(getRandomInt(50,80));
    }
}


function sendData(d)
{
  io.sockets.emit('hr', d);
  console.log(d);
}

function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function getRandomInt(min, max)
{
    return Math.floor(Math.random()*(max-min))+min;
}

main();