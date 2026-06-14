/// <reference path="_references.js" />

function validateMobilePhone(mobilePhone) {
	var regex = /^[0-9\-]{0,11}$/;

	return (regex.test(mobilePhone));
}

function validateEmail(email) {
	var regex = /^[\w_@.\-]{0,50}$/;

	return (regex.test(email));
}

function validateNumbers(number) {
	var regex = /^[0-9]{0,9}$/;

	return (regex.test(number));
}

function validateLoadMoney(number) {
	var regex = /^[1-9][0-9]*$/;
	return (regex.test(number));
}

function validatePassword(password) {
	var regex = /^[\s|?!@#$%^&*0-9A-Za-zא-ת.,_]{0,20}$/;

	return (regex.test(password));
}

function validateCaptcha(captcha) {
	var regex = /^[0-9A-Za-z ]{0,6}$/;

	return (regex.test(captcha));
}

function validateNames(name) {
	var regex = /^[\sA-Zא-תa-z'-]{0,20}$/;

	return (regex.test(name));
}

function validateAddress(address) {
	var regex = /^[\s0-9A-Zא-תa-z,'\-\/\"]{0,30}$/;

	return (regex.test(address));
}

function validateDate(date) {
	var regex = /^[0-9-/\s]{0,10}$/;

	return (regex.test(date));
}

function validateSearchText(searchText) {
	var regex = /^[\s?!0-9A-Zא-תa-z'"\-]{0,20}$/;

	return (regex.test(searchText));
}

function validateCommentText(commentText) {
	var regex = /^[\s0-9A-Zא-תa-z]{0,20}$/;

	return (regex.test(commentText));
}

function validateFinalCommentText(commentText) {
	var regex = /^[\s0-9A-Zא-תa-z]{3,20}$/;

	return (regex.test(commentText));
}

function validateFinalSearchText(searchText) {
	var regex = /^[\s?!0-9A-Zא-תa-z'"\-]{3,20}$/;

	return (regex.test(searchText));
}

function validateFinalFirstName(isReq) {
	var regex = isReq ? /^[\sA-Zא-תa-z'-]{2,20}$/ : /^[\sA-Zא-תa-z'-]{0,20}$/;

	var fname = $('#txtFirstName');
	var name = fname.val();

	var isValid = regex.test(name)

	var divForAlert = fname.parent().children('.clsAlert');
	var spanAlert = divForAlert.children('.clsErrorMessages');

	if (isValid) {
		divForAlert.hide();
	} else if (isReq) {
		spanAlert.text('שם פרטי הינו חובה');
		divForAlert.show();
	}

	return (isValid);
}

function validateFinalLastName(isReq) {
	var regex = isReq ? /^[\sA-Zא-תa-z'-]{2,20}$/ : /^[\sA-Zא-תa-z'-]{0,20}$/;

	var lname = $('#txtLastName');
	var name = lname.val();

	var isValid = regex.test(name);

	var divForAlert = lname.parent().children('.clsAlert');
	var spanAlert = divForAlert.children('.clsErrorMessages');

	if (isValid) {
		divForAlert.hide();
	} else if (isReq) {
		spanAlert.text('שם משפחה הינו חובה');
		divForAlert.show();
	}

	return (isValid);
}

function validateFinalAddress(isReq) {
	var regex = isReq ? /^[\s0-9A-Zא-תa-z,'\-\/\"]{2,30}$/ : /^[\s0-9A-Zא-תa-z,'\-\/\"]{0,30}$/;

	var address = $('#txtAddress');
	var value = $('#txtAddress').val();

	var isValid = regex.test(value);

	var divForAlert = address.parent().children('.clsAlert');
	var spanAlert = divForAlert.children('.clsErrorMessages');

	if (isValid) {
		divForAlert.hide();
	} else if (isReq) {
		spanAlert.text('שדה כתובת הינו חובה');
		divForAlert.show();
	}

	return (isValid);
}

function validateFinalBirthdate() {
	var regex = /^[0-9\-/\s]{0,10}$/;

	var birthdate = $('#txtBirthdate').val();

	return (regex.test(birthdate));
}

function validateFinalMemberId() {
	var regex = /^[0-9]{4,9}$/;

	var memberId = $('#txtMemberId').val();

	return (regex.test(memberId));
}

function validateFinalMemberId(checkId) {
	var regex = /^[0-9]{4,9}$/;

	var memberId = $('#txtMemberId').val();

	if (checkId) {
		return (validationForId(memberId) && regex.test(memberId));
	}

	return (regex.test(memberId));
}

function validateFinalOtherMemberId() {
	var regex = /^[0-9]{4,9}$/;

	var memberId = $('#txtOtherMemberId').val();

	return (regex.test(memberId));
}

function validateFinalTZnumber() {
	var regex = /^[0-9]{4,9}$/;

	var TZnumber = $('#txtTZnumber');
	var value = TZnumber.val();
	var isValid = regex.test(value);

	var divForAlert = TZnumber.parent().children('.clsAlert');
	var spanAlert = divForAlert.children('.clsErrorMessages');

	if (isValid) {
		divForAlert.hide();
	} else {
		spanAlert.text('שדה מספר תעודת זהות הינו חובה');
		divForAlert.show();
	}

	return (isValid);
}

function validateFinalMobilePhone() {
	debugger;
	var regex = /^\(?(0[0-9]{2})\)?[\-. ]?([0-9]{3})[\-. ]?([0-9]{4})$/;

	var mobilePhone = $('#txtMobileNumber');
	var value = mobilePhone.val();
	var isValid = regex.test(value);

	var divForAlert = mobilePhone.parent().children('.clsAlert');
	var spanAlert = divForAlert.children('.clsErrorMessages');

	if (isValid) {
		divForAlert.hide();
	} else {
		spanAlert.text('שדה מספר סלולרי הינו חובה');
		divForAlert.show();
	}

	return (isValid);
}

function validateFinalEmail() {
	var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

	var email = $('#txtEmail');
	var value = email.val();
	var isValid = regex.test(value);

	var divForAlert = email.parent().children('.clsAlert');
	var spanAlert = divForAlert.children('.clsErrorMessages');

	if (isValid) {
		divForAlert.hide();
	} else {
		spanAlert.text('שדה דוא"ל הינו חובה, ודא כי הזנת אותו באופן חוקי');
		divForAlert.show();
	}

	return (isValid);
}

function validateFinalPassword() {
	var regex = /^[\s|?!@#$%^&*0-9A-Za-zא-ת.,_]{8,20}$/;

	var password = $('#txtPassword').val();

	return (regex.test(password));
}

function validateFinalLastNumbersOfCard() {
	var regex = /^[0-9]{8}$/;

	var lastNumbersOfCard = $('#txtLastNumbersOfCard').val();

	return (regex.test(lastNumbersOfCard));
}

function validateFinalCaptcha() {
	var regex = /^[0-9A-Za-z ]{5,6}$/;

	var captcha = $('#txtCaptcha').val();

	return (regex.test(captcha));
}

function validateFinalLoadMoney() {
	var regex = /^[1-9][0-9]*$/;
	var loadMoney = $('#ddlLoadSteps').val();
	//alert(loadMoney);
	return (regex.test(loadMoney));
}

function validateId(id) {
	var count = 0;

	if (id < 9) {
		id = ("000000000" + id).slice(-9);
	}
	for (var i = 0; i < 8; i++) {
		var x = (((i % 2) + 1) * id.charAt(i));

		if (x > 9) {
			x = x.toString();
			x = parseInt(x.charAt(0)) + parseInt(x.charAt(1));
		}
		count += x;
	}

	if ((count + parseInt(id.charAt(8))) % 10 === 0) {
		return true;
	} else {
		return false;
	}
}

function validateId2(id) {
	var IDnum = String(id);
	if (isNaN(IDnum))
		return false;
	if (IDnum.length < 9) {
		while (IDnum.length < 9) {
			IDnum = '0' + IDnum;
		}
	}
	var mone = 0, incNum;
	for (var i = 0; i < 9; i++) {
		incNum = Number(IDnum.charAt(i));
		incNum *= (i % 2) + 1;
		if (incNum > 9)
			incNum -= 9;
		mone += incNum;
	}
	return (mone % 10 === 0);

}

function validatePhoneNumber(phone, valueWithPrefix) {
			
	valueWithPrefix = typeof valueWithPrefix === 'undefined' ? true : valueWithPrefix;
	var regex;
	if (valueWithPrefix)
		regex = new RegExp(/^(0\d{1,2}[2-9]{1}\d{6})$/);
	else
		regex = new RegExp(/^([2-9]{1}\d{6})$/);

	if (regex.test(phone)) {
		return true;
	}
	return false;
}

function validateNotXSS(str) {
	if (typeof str !== undefined && str !== null && str.length > 0) {
		var regex = [];
		regex.push('<[^\w<>]*(?:[^<>"\'');
		regex.push('\s]*:)?[^\w<>]*(?:\W*s\W*c\W*r\W*i\W*p\W*t|\W*f\W*o\W*r\W*m|\W*s\W*t\W*y\W*l\W*e|\W*s\W*v\W*g|\W*m\W*a\W*r\W*q\W*u\W*e\W*e|');
		regex.push('(?:\W*l\W*i\W*n\W*k|\W*o\W*b\W*j\W*e\W*c\W*t|\W*e\W*m\W*b\W*e\W*d|\W*a\W*p\W*p\W*l\W*e\W*t|\W*p\W*a\W*r\W*a\W*m|\W*i?');
		regex.push('\W*f\W*r\W*a\W*m\W*e|\W*b\W*a\W*s\W*e|\W*b\W*o\W*d\W*y|\W*m\W*e\W*t\W*a|\W*i\W*m\W*a?\W*g\W*e?|\W*v\W*i\W*d\W*e\W*o|\W*a\W*u\W*d\W*i\W*o|');
		regex.push('\W*b\W*i\W*n\W*d\W*i\W*n\W*g\W*s|\W*s\W*e\W*t|\W*i\W*s\W*i\W*n\W*d\W*e\W*x|\W*a\W*n\W*i\W*m\W*a\W*t\W*e)[^>\w])');
		regex.push('|(?:<\w[\s\S]*[\s\0\/]|[\'');
		regex.push('"])(?:formaction|style|background|src|lowsrc|ping|on(?:d(?:e(?:vice(?:(?:orienta|mo)tion|proximity|found|light)');
		regex.push('|livery(?:success|error)|activate)|r(?:ag(?:e(?:n(?:ter|d)|xit)|(?:gestur|leav)e|start|drop|over)?|op)|i(?:s(?:c(?:hargingtimechange|onnect');
		regex.push('(?:ing|ed))|abled)|aling)|ata(?:setc(?:omplete|hanged)|(?:availabl|chang)e|error)|urationchange|ownloading|blclick)|Moz(?:M(?:agnifyGesture');
		regex.push('(?:Update|Start)?|ouse(?:PixelScroll|Hittest))|S(?:wipeGesture(?:Update|Start|End)?|crolledAreaChanged)|(?:(?:Press)?TapGestur|BeforeResiz)e|');
		regex.push('EdgeUI(?:C(?:omplet|ancel)|Start)ed|RotateGesture(?:Update|Start)?|A(?:udioAvailable|fterPaint))|c(?:o(?:m(?:p(?:osition(?:update|start|end)|lete)');
		regex.push('|mand(?:update)?)|n(?:t(?:rolselect|extmenu)|nect(?:ing|ed))|py)|a(?:(?:llschang|ch)ed|nplay(?:through)?|rdstatechange)|h(?:(?:arging(?:time)?ch)');
		regex.push('?ange|ecking)|(?:fstate|ell)change|u(?:echange|t)|l(?:ick|ose))|m(?:o(?:z(?:pointerlock(?:change|error)|(?:orientation|time)change|fullscreen');
		regex.push('(?:change|error)|network(?:down|up)load)|use(?:(?:lea|mo)ve|o(?:ver|ut)|enter|wheel|down|up)|ve(?:start|end)?)|essage|ark)|s(?:t(?:a(?:t(?:uschanged');
		regex.push('|echange)|lled|rt)|k(?:sessione|comma)nd|op)|e(?:ek(?:complete|ing|ed)|(?:lec(?:tstar)?)?t|n(?:ding|t))|u(?:ccess|spend|bmit)|peech(?:start|end)|ound');
		regex.push('(?:start|end)|croll|how)|b(?:e(?:for(?:e(?:(?:scriptexecu|activa)te|u(?:nload|pdate)|p(?:aste|rint)|c(?:opy|ut)|editfocus)|deactivate)|gin(?:Event)?)');
		regex.push('|oun(?:dary|ce)|l(?:ocked|ur)|roadcast|usy)|a(?:n(?:imation(?:iteration|start|end)|tennastatechange)|fter(?:(?:scriptexecu|upda)te|print)|udio');
		regex.push('(?:process|start|end)|d(?:apteradded|dtrack)|ctivate|lerting|bort)|DOM(?:Node(?:Inserted(?:IntoDocument)?|Removed(?:FromDocument)?)|');
		regex.push('(?:CharacterData|Subtree)Modified|A(?:ttrModified|ctivate)|Focus(?:Out|In)|MouseScroll)|r(?:e(?:s(?:u(?:m(?:ing|e)|lt)|ize|et)|adystatechange|pea');
		regex.push('(?:tEven)?t|movetrack|trieving|ceived)|ow(?:s(?:inserted|delete)|e(?:nter|xit))|atechange)|p(?:op(?:up(?:hid(?:den|ing)|show(?:ing|n))|state)|a');
		regex.push('(?:ge(?:hide|show)|(?:st|us)e|int)|ro(?:pertychange|gress)|lay(?:ing)?)|t(?:ouch(?:(?:lea|mo)ve|en(?:ter|d)|cancel|start)|ime(?:update|out)');
		regex.push('|ransitionend|ext)|u(?:s(?:erproximity|sdreceived)|p(?:gradeneeded|dateready)|n(?:derflow|load))|f(?:o(?:rm(?:change|input)|cus(?:out|in)?)');
		regex.push('|i(?:lterchange|nish)|ailed)|l(?:o(?:ad(?:e(?:d(?:meta)?data|nd)|start)?|secapture)|evelchange|y)|g(?:amepad(?:(?:dis)?connected|button(?:down|up)|axismove)');
		regex.push('|et)|e(?:n(?:d(?:Event|ed)?|abled|ter)|rror(?:update)?|mptied|xit)|i(?:cc(?:cardlockerror|infochange)|n(?:coming|valid|put))|o(?:(?:(?:ff|n)lin|bsolet)e|verflow');
		regex.push('(?:changed)?|pen)|SVG(?:(?:Unl|L)oad|Resize|Scroll|Abort|Error|Zoom)|h(?:e(?:adphoneschange|l[dp])|ashchange|olding)|v(?:o(?:lum|ic)e|ersion)change');
		regex.push('|w(?:a(?:it|rn)ing|heel)|key(?:press|down|up)|(?:AppComman|Loa)d|no(?:update|match)|Request|zoom))[\s\0]*=');

		return new RegExp(regex.join('')).test(str) === false;
	}
	return true;
}

$(document).ready(function () {
	$('.idNumValidator').blur(function (e) {
		if (!validateId2($(this).val())) {
			alert("illegal id number");
			//Write your code here
		}
	});

	$('.phonenumberValidator').blur(function (e) {
		if (!validatePhoneNumber($(this).val())) {
			alert("illegal phone number");
			//Write your code here
		}
	});

	$('.notXSSValidator').blur(function (e) {
		if (!validateNotXSS($(this).val())) {
			alert("XSS script is illegal");
			//Write your code here
		}
	});
});