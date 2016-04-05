
function isEmailAddress(val) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test(val);
}
function isInteger(val)
{
    var regexp = new RegExp('^\\d+$');
    return regexp.test(val);
}

function isDecimal(val) {
    var regexp = /^\d+\.\d{0,2}$/;
    return regexp.test(val);
}
