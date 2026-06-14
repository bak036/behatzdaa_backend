/// <reference path="_references.js" />

//-------------------------------- include JavaScript files --------------------------------
//document.write('<script src="http://code.jquery.com/jquery-2.2.4.min.js" type="text/javascript"></script>');
//------------------------------------------------------------------------------------------

function isEmpty(val) {
	return (val === undefined || val == null || val.length <= 0) ? true : false;
}

//////////////////////////////////////////////////////////////////////////////////////
//// Check if browser is IE and return its version or return 0 for other browsers ////
//////////////////////////////////////////////////////////////////////////////////////
function isIE() {
	var browser = window.navigator.userAgent.toLowerCase();
	return ( browser.indexOf( 'msie' ) != -1 ) ? parseInt( browser.split( 'msie' )[1] ) : ( browser.indexOf( 'trident' ) != -1 ) ? 11 : 0;
}

function trim(input) {
	var temp = input.match(/^\s*(\S+(\s+\S+)*)\s*$/);
	return (temp == null) ? "" : temp[1];
}

//function trim(input) {
//    var temp = input;
//    var lre = /^\s*/;
//    var rre = /\s*$/;
//    temp = temp.replace(lre, "");
//    temp = temp.replace(rre, "");
//    return temp;
//}

//////////////////////////////////////////////////////////////////////////////////////
//// Extension functions for String type                                          ////
//////////////////////////////////////////////////////////////////////////////////////

if (typeof String.prototype.trim !== 'function') {
	String.prototype.trim = function () {
		return this.replace(/^\s+|\s+$/g, '');
	}
}

String.prototype.__trim = function () {
	var temp = this.match(/^\s*(\S+(\s+\S+)*)\s*$/);
	return (temp == null) ? '' : temp[1];
}

if (typeof String.prototype.trimStart !== 'function') {
	String.prototype.trimStart = function () {
		return this.replace(/^\s*/, '');
	}
}

String.prototype.__trimStart = function () {
	return this.replace(/^\s*/, '');
}

if (typeof String.prototype.trimEnd !== 'function') {
	String.prototype.trimEnd = function () {
		return this.replace(/\s*$/, '');
	}
}

String.prototype.__trimEnd = function () {
	return this.replace(/\s*$/, '');
}

if (typeof String.prototype.trimAll !== 'function') {
	String.prototype.trimAll = function () {
		return this.replace(/\s*/g, '');
	}
}

String.prototype.__trimAll = function () {
	return this.replace(/\s*/g, '');
}

if (typeof String.prototype.reverse !== 'function') {
	String.prototype.reverse = function (separator) {
		return this.split(separator).reverse().join(separator);
	}
}

String.prototype.__reverse = function (separator) {
	return this.split(separator).reverse().join(separator);
}

//if (typeof String.prototype.trim !== 'function') {
//	String.prototype.trim = function () {
//		var temp = this.match(/^\s*(\S+(\s+\S+)*)\s*$/);
//		return (temp == null) ? '' : temp[1];
//	}
//}

//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

function directoryExist(path) {
	if ((path.indexOf("\\") > -1) && (path.indexOf("\\fakepath\\") == -1)) // Directory exist in path
		return true;
	return false;
}

function getObjectLength(jsonObject) {
	return (Object.keys(jsonObject).length);
}

function swap2ArrayDivs(firstArrDiv, secondArrDiv) {
	for (var i = 0; i < firstArrDiv.length; i++) {
		var temp = firstArrDiv[i].outerHTML;
		firstArrDiv[i].outerHTML = secondArrDiv[i].outerHTML;
		secondArrDiv[i].outerHTML = temp;
	}
}

function routeToPath(url) {
	location.href = "/" + url;
}

function createCookie(name, value, days) {
	var expires;
	if (days) {
		var date = new Date();
		date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
		expires = "; expires=" + date.toGMTString();
	}
	else {
		expires = "";
	}
	document.cookie = name + "=" + value + expires + "; path=/";
}

function getCookie(c_name) {
	if (document.cookie.length > 0) {
		c_start = document.cookie.indexOf(c_name + "=");
		if (c_start != -1) {
			c_start = c_start + c_name.length + 1;
			c_end = document.cookie.indexOf(";", c_start);
			if (c_end == -1) {
				c_end = document.cookie.length;
			}
			return unescape(document.cookie.substring(c_start, c_end));
		}
	}
	return "";
}

function getReverseArray(__array) {
	var reverseArray = new Array(__array.length);
	var z = __array.length - 1;

	for (i = 0; i < __array.length; i++) {
		reverseArray[z--] = __array[i];
	}
	return reverseArray;
}

function getCurrentDateString() {
	var _date = new Date();
	var day = (_date.getDate().toString().length < 2) ? ("0" + _date.getDate()) : _date.getDate();
	var month = (_date.getMonth().toString().length < 2) ? ("0" + (_date.getMonth() + 1)) : (_date.getMonth() + 1);
	var year = _date.getFullYear();

	return new String(day + '/' + month + '/' + year);
}

function getControlById(ctrl) {
	var pattern = new RegExp(ctrl + "$");
	var controls = document.getElementsByTagName('*');
	var i;

	for (i = 0; i < controls.length; i++) {
		if (controls[i].id.search(pattern) > -1) {
			return controls[i];
		}
	}
	return null;
}

//////////////////////////////////////////////////////////////////////////////////////
//// Utility functions for WebForm validators                                     ////
//////////////////////////////////////////////////////////////////////////////////////

function getValidatorById(val) {
	if (typeof (Page_Validators) != "undefined") {
		if ((Page_Validators != null) && (Page_Validators.length > 0)) {
			var pattern = new RegExp(val + "$");
			var i;

			for (i = 0; i < Page_Validators.length; i++)
				if (Page_Validators[i].id.search(pattern) > -1)
					return Page_Validators[i];
		}
	}
	return null;
}

function findControlValidators(ctrl) {
	if (typeof (Page_Validators) !== "undefined") {
		if ((Page_Validators !== null) && (Page_Validators.length > 0)) {
			var validators = new Array();
			var z = 0;
			var i;
			for (i = 0; i < Page_Validators.length; i++) {
				if (Page_Validators[i].controltovalidate === ctrl.id)
					validators[z++] = Page_Validators[i];
			}
			return validators;
		}
	}
	return null;
}

function getControlValidators(ctrl) {
	var vals;
	if (typeof (ctrl.Validators) !== "undefined") {
		vals = ctrl.Validators;
	}
	else if ((typeof (ctrl.tagName) !== "undefined") && (ctrl.tagName.toLowerCase() === "label")) {
		ctrl = document.getElementById(ctrl.htmlFor);
		vals = ctrl.Validators;
	}
	else {
		vals = findControlValidators(ctrl);
	}
	return vals;
}

function disableControlValidators(ctrl) {
	var vals = getControlValidators(ctrl);

	if ((vals !== null) && (vals.length > 0)) {
		var i;
		for (i = 0; i < vals.length; i++) {
			ValidatorEnable(vals[i], false);
			//vals[i].style.visibility = "hidden";
			//vals[i].isvalid = true;
		}
	}
}

//function findValidationGroupValidators(ctrl) {
//	if (typeof (Page_Validators) !== "undefined") {
//		if ((Page_Validators !== null) && (Page_Validators.length > 0)) {
//			var validators = new Array();
//			var z = 0;
//			var i;
//			for(i = 0; i < Page_Validators.length; i++) {
//				if(Page_Validators[i].validationGroup === ctrl.validationGroup) {
//					validators[z++] = Page_Validators[i];
//				}
//			}
//			return validators;
//		}
//	}
//	return null;
//}

//function disableValidationGroupValidators(ctrl) {
//	var vals = findValidationGroupValidators(ctrl);
//
//	if ((vals !== null) && (vals.length > 0)) {
//		var i;
//		for (i = 0; i < vals.length; i++) {
//			ValidatorEnable(vals[i], false);
//          //vals[i].style.visibility = "hidden";
//          //vals[i].isvalid = true;
//		}
//	}
//}

//function hideControlValidators(ctrl) {
//	var vals;
//	if (typeof (ctrl.Validators) != "undefined") {
//		vals = ctrl.Validators;
//	}
//	else if ((typeof (ctrl.tagName) != "undefined") && (ctrl.tagName.toLowerCase() == "label")) {
//		ctrl = document.getElementById(ctrl.htmlFor);
//		vals = ctrl.Validators;
//	}
//	else {
//		vals = FindControlValidators(ctrl);
//	}
//
//	if ((vals != null) && (vals.length > 0)) {
//		var i;
//		for (i = 0; i < vals.length; i++) {
//			vals[i].style.visibility = "hidden";
//			vals[i].isvalid = true;
//		}
//	}
//}

//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

(function ($) {
	$.fn.getHiddenDimensions = function (includeMargin) {
		var $item = this,
		props = { position: 'absolute', visibility: 'hidden', display: 'block' },
		dim = { width: 0, height: 0, innerWidth: 0, innerHeight: 0, outerWidth: 0, outerHeight: 0 },
		$hiddenParents = $item.parents().andSelf().not(':visible'),
		includeMargin = (includeMargin == null) ? false : includeMargin;

		var oldProps = [];
		$hiddenParents.each(function () {
			var old = {};

			for (var name in props) {
				old[name] = this.style[name];
				this.style[name] = props[name];
			}

			oldProps.push(old);
		});

		dim.width = $item.width();
		dim.outerWidth = $item.outerWidth(includeMargin);
		dim.innerWidth = $item.innerWidth();
		dim.height = $item.height();
		dim.innerHeight = $item.innerHeight();
		dim.outerHeight = $item.outerHeight(includeMargin);

		$hiddenParents.each(function (i) {
			var old = oldProps[i];
			for (var name in props) {
				this.style[name] = old[name];
			}
		});

		return dim;
	}
}(jQuery));