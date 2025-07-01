/**
* Universidad de La Laguna
* Proyecto: Roblockly-Android
* Autor: Thomas Edward Bradley
* Email: alu0101408248@ull.edu.es
* Fecha: 01/07/2025
* Descripcion: Plugin para pergar de clipboard en WebGL
*/

mergeInto(LibraryManager.library, {
  PasteFromClipboard: function () {
    // This triggers a browser paste event for a focused input
    navigator.clipboard.readText().then(function(text) {
      window.unityInstance.SendMessage('ClipboardManager', 'OnClipboardPaste', text); // Custom WebGL Template necesario para esto
    }).catch(function(err) {
      console.error('Failed to read clipboard: ', err);
    });
  }
});
