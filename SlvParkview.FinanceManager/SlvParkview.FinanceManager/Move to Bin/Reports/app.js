"use strict";

console.log("Hello, from app.js");

var jsonDataFilename = getFilename(document.baseURI).replace("html", "json");

console.log(jsonDataFilename);

import jsonData from jsonDataFilename assert {type: 'json'};

console.log(jsonData);