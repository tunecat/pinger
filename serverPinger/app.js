// app.js
var express = require("express");
var statusCode = 200;

var app = express();
app.set("port", 8080);

app.listen(app.get("port"), function () {
  console.log("Node app is running on port", app.get("port"));
});

app.get("/", (req, res) => {
  testPingingFailure(res);
  testPingingSuccessfull(res);

  testPingingSuccessAfterFailure(res);
  console.log(statusCode);
});

function testPingingSuccessfull(res) {
  res.sendStatus(200);
  statusCode = 200;
}

function testPingingFailure(res) {
  res.sendStatus(401);
  statusCode = 401;
}

// times pinged
var n = 0;
function testPingingSuccessAfterFailure(res) {
  if (n < 12) {
    testPingingFailure(res);
    statusCode = 401;
    n++;
  } else {
    testPingingSuccessfull(res);
    statusCode = 200;
    n = 0;
  }
}
