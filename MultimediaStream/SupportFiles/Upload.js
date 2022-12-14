
	'use strict';

; (function ($, window, document, undefined) {


	var isAdvancedUpload = function () {
		var div = document.createElement('div');
		return (('draggable' in div) || ('ondragstart' in div && 'ondrop' in div)) && 'FormData' in window && 'FileReader' in window;
	}();



	$('.box').each(function () {
		var $form = $(this),
			$input = $form.find('input[type="file"]'),
			$label = $form.find('label'),
			$errorMsg = $form.find('.box-error span'),
			$restart = $form.find('.box-restart'),
			droppedFiles = false,
			showFiles = function (files) {
				$label.text(files.length > 1 ? ($input.attr('data-multiple-caption') || '').replace('{count}', files.length) : files[0].name);
			};


		$form.append('<input type="hidden" name="ajax" value="1" />');


		$input.on('change', function (e) {
			showFiles(e.target.files);


		});


		
		if (isAdvancedUpload) {
			$form
				.addClass('has-advanced-upload') 
				.on('drag dragstart dragend dragover dragenter dragleave drop', function (e) {
				
					e.preventDefault();
					e.stopPropagation();
				})
				.on('dragover dragenter', function () 
				{
					$form.addClass('is-dragover');
				})
				.on('dragleave dragend drop', function () {
					$form.removeClass('is-dragover');
				})
				.on('drop', function (e) {
					droppedFiles = e.originalEvent.dataTransfer.files; // the files that were dropped
					showFiles(droppedFiles);


				});
		}




		$form.on('submit', function (e) {
			if ($form.hasClass('is-uploading')) return false;

			$form.addClass('is-uploading').removeClass('is-error');

			if (isAdvancedUpload) 
			{
				e.preventDefault();

				var ajaxData = new FormData($form.get(0));
				if (droppedFiles) {
					$.each(droppedFiles, function (i, file) {
						ajaxData.append($input.attr('name'), file);
					});
				}

				var name = $("#name").text().trim();
				
				$.ajax(

					{
						url: "/api/upload",
						type: "POST",
						headers: { 'username': name },
						data: ajaxData,
						dataType: 'json',
						cache: false,
						contentType: false,
						processData: false,
						complete: function () {
							$form.removeClass('is-uploading');
						},
						success: function (data) {
							//$form.addClass(data.success == true ? 'is-success' : 'is-error');
							//if (!data.success) $errorMsg.text(data.error);
							alert("Upload Sucess")
						},
						error: function () {
							alert('Error. Please, contact the webmaster!');
						}
					});
			}
			else 
			{
				var iframeName = 'uploadiframe' + new Date().getTime(),
					$iframe = $('<iframe name="' + iframeName + '" style="display: none;"></iframe>');

				$('body').append($iframe);
				$form.attr('target', iframeName);

				$iframe.one('load', function () {
					var data = $.parseJSON($iframe.contents().find('body').text());
					$form.removeClass('is-uploading').addClass(data.success == true ? 'is-success' : 'is-error').removeAttr('target');
					if (!data.success) $errorMsg.text(data.error);
					$iframe.remove();
				});
			}
		});



		$restart.on('click', function (e) {
			e.preventDefault();
			$form.removeClass('is-error is-success');
			$input.trigger('click');
		});

		$input
			.on('focus', function () { $input.addClass('has-focus'); })
			.on('blur', function () { $input.removeClass('has-focus'); });
	});

})(jQuery, window, document);



	'use strict';

; (function (document, window, index) {
	var isAdvancedUpload = function () {
		var div = document.createElement('div');
		return (('draggable' in div) || ('ondragstart' in div && 'ondrop' in div)) && 'FormData' in window && 'FileReader' in window;
	}();


	var forms = document.querySelectorAll('.box');
	Array.prototype.forEach.call(forms, function (form) {
		var input = form.querySelector('input[type="file"]'),
			label = form.querySelector('label'),
			errorMsg = form.querySelector('.box-error span'),
			restart = form.querySelectorAll('.box-restart'),
			droppedFiles = false,
			showFiles = function (files) {
				label.textContent = files.length > 1 ? (input.getAttribute('data-multiple-caption') || '').replace('{count}', files.length) : files[0].name;
			},
			triggerFormSubmit = function () {
				var event = document.createEvent('HTMLEvents');
				event.initEvent('submit', true, false);
				form.dispatchEvent(event);
			};

		var ajaxFlag = document.createElement('input');
		ajaxFlag.setAttribute('type', 'hidden');
		ajaxFlag.setAttribute('name', 'ajax');
		ajaxFlag.setAttribute('value', 1);
		form.appendChild(ajaxFlag);

		input.addEventListener('change', function (e) {
			showFiles(e.target.files);


		});

		if (isAdvancedUpload) {
			form.classList.add('has-advanced-upload'); 

			['drag', 'dragstart', 'dragend', 'dragover', 'dragenter', 'dragleave', 'drop'].forEach(function (event) {
				form.addEventListener(event, function (e) {
					// preventing the unwanted behaviours
					e.preventDefault();
					e.stopPropagation();
				});
			});
			['dragover', 'dragenter'].forEach(function (event) {
				form.addEventListener(event, function () {
					form.classList.add('is-dragover');
				});
			});
			['dragleave', 'dragend', 'drop'].forEach(function (event) {
				form.addEventListener(event, function () {
					form.classList.remove('is-dragover');
				});
			});
			form.addEventListener('drop', function (e) {
				droppedFiles = e.dataTransfer.files; // the files that were dropped
				showFiles(droppedFiles);

			});
		}


		form.addEventListener('submit', function (e) {
			if (form.classList.contains('is-uploading')) return false;

			form.classList.add('is-uploading');
			form.classList.remove('is-error');

			if (isAdvancedUpload) 
			{
				e.preventDefault();

				var ajaxData = new FormData(form);
				if (droppedFiles) {
					Array.prototype.forEach.call(droppedFiles, function (file) {
						ajaxData.append(input.getAttribute('name'), file);
					});
				}

				var ajax = new XMLHttpRequest();
				ajax.open(form.getAttribute('method'), form.getAttribute('action'), true);

				ajax.onload = function () {
					form.classList.remove('is-uploading');
					if (ajax.status >= 200 && ajax.status < 400) {
						var data = JSON.parse(ajax.responseText);
						form.classList.add(data.success == true ? 'is-success' : 'is-error');
						if (!data.success) errorMsg.textContent = data.error;
					}
					else alert('Error. Please, contact the webmaster!');
				};

				ajax.onerror = function () {
					form.classList.remove('is-uploading');
					alert('Error. Please, try again!');
				};

				ajax.send(ajaxData);
			}
			else 
			{
				var iframeName = 'uploadiframe' + new Date().getTime(),
					iframe = document.createElement('iframe');

				$iframe = $('<iframe name="' + iframeName + '" style="display: none;"></iframe>');

				iframe.setAttribute('name', iframeName);
				iframe.style.display = 'none';

				document.body.appendChild(iframe);
				form.setAttribute('target', iframeName);

				iframe.addEventListener('load', function () {
					var data = JSON.parse(iframe.contentDocument.body.innerHTML);
					form.classList.remove('is-uploading')
					form.classList.add(data.success == true ? 'is-success' : 'is-error')
					form.removeAttribute('target');
					if (!data.success) errorMsg.textContent = data.error;
					iframe.parentNode.removeChild(iframe);
				});
			}
		});


		Array.prototype.forEach.call(restart, function (entry) {
			entry.addEventListener('click', function (e) {
				e.preventDefault();
				form.classList.remove('is-error', 'is-success');
				input.click();
			});
		});

		input.addEventListener('focus', function () { input.classList.add('has-focus'); });
		input.addEventListener('blur', function () { input.classList.remove('has-focus'); });

	});
}(document, window, 0));
