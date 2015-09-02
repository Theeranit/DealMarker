//Start encode process
function encodePass(pass) {
    pass = generateSalt() + pass + generateSalt();
    pass = insertNthChar(pass, '+ +', 1);
    pass = encodeURIComponent(pass);
    return pass;

}

function insertNthChar(string, chr, nth) {
    var output = '';
    for (var i = 0; i < string.length; i++) {
        if (i > 0 && i % 1 == 0)
            output += chr;
        output += string.charAt(i);
    }

    return output;
}

function generateSalt() {

    var length = 4;
    var sSalt = "";

    var noPunction = true;

    for (i = 0; i < length; i++) {

        numI = getRandomNum();
        if (noPunction) { while (checkPunc(numI)) { numI = getRandomNum(); } }

        sSalt = sSalt + String.fromCharCode(numI);
    }

    return sSalt;

}

function getRandomNum() {

    // between 0 - 1
    var rndNum = Math.random()

    // rndNum from 0 - 1000    
    rndNum = parseInt(rndNum * 1000);

    // rndNum from 33 - 127        
    rndNum = (rndNum % 94) + 33;

    return rndNum;
}

function checkPunc(num) {

    if ((num >= 33) && (num <= 47)) { return true; }
    if ((num >= 58) && (num <= 64)) { return true; }
    if ((num >= 91) && (num <= 96)) { return true; }
    if ((num >= 123) && (num <= 126)) { return true; }

    return false;
}