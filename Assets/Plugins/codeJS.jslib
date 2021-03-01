mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
  },

  HelloString: function (str) {
	window.alert(Pointer_stringify(str));
  },  
  
  FillCode1: function (str) {
	document.getElementById('code1').value = Pointer_stringify(str);
  },  
  FillCode2: function (str) {
	document.getElementById('code2').value = Pointer_stringify(str);
  },  
  FillCode3: function (str) {
	document.getElementById('code3').value = Pointer_stringify(str);
  }, 
  FileName1: function (str) {
	document.getElementById('code1_nom').value = Pointer_stringify(str);
  },
  FileName2: function (str) {
	document.getElementById('code2_nom').value = Pointer_stringify(str);
  },
  FileName3: function (str) {
	document.getElementById('code3_nom').value = Pointer_stringify(str);
  },
});