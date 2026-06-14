/// <reference path="_references.js" />

$(document).ready(function () {
	// utility jQuery plugins
	$.fn.blockInput = function (options) {
		// find inserted or removed characters
		function findDelta(value, prevValue) {
			var delta = '';

			for (var i = 0; i < value.length; i++) {
				var str = value.substr(0, i) +
					value.substr(i + value.length - prevValue.length);

				if (str === prevValue) delta =
					value.substr(i, value.length - prevValue.length);
			}

			return delta;
		}

		function isValidChar(c) {
			return new RegExp(options.regex).test(c);
		}

		function isValidString(str) {
			for (var i = 0; i < str.length; i++)
				if (!isValidChar(str.substr(i, 1))) return false;

			return true;
		}

		this.filter('input,textarea').on('input', function () {
			var val = this.value,
				lastVal = $(this).data('lastVal');

			// get inserted chars
			var inserted = findDelta(val, lastVal);
			// get removed chars
			var removed = findDelta(lastVal, val);
			// determine if user pasted content
			var pasted = inserted.length > 1 || (!inserted && !removed);

			if (pasted) {
				if (!isValidString(val)) this.value = lastVal;
			}
			else if (!removed) {
				if (!isValidChar(inserted)) this.value = lastVal;
			}

			// store current value as last value
			$(this).data('lastVal', this.value);
		}).on('focus', function () {
			$(this).data('lastVal', this.value);
		});

		return this;
	};

	$('.onlyNumbers').blockInput({ regex: '[0-9]+$' });
	$('.onlyText').blockInput({ regex: '^[a-zA-Zא-ת\x20]+$' });




});