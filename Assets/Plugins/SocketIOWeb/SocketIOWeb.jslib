mergeInto(LibraryManager.library, {

	SocketIO_Init: function(name) {
		window.gameObjectName = Pointer_stringify(name);
		
		// Free pointer
        // _free(name)
	},

	SocketIO_Connect: function() {
		/*
		const socket = io(window.location, { forceNew: true });

		const onevent = socket.onevent;
		socket.onevent = function (packet) {
			let args = packet.data || [];
			onevent.call (this, packet);    // original call
			packet.data = ['*'].concat(args);
			onevent.call(this, packet);      // additional call to catch-all
		};

		socket.on('*', function(e, args) {
			unityInstance.SendMessage(window.gameObjectName, 'OnEvent', e, args);
		});

		socket.on('disconnect', function() {
			unityInstance.SendMessage(window.gameObjectName, 'OnDisconnect');
		});

		window.socket = socket;
		 */

		// Use window delegate
		window.SocketIOUnityConnect(unityInstance);
	},

	SocketIO_Emit: function(e, data) {
		/*
		window.socket.emit(e, data);d
		 */
		// console.log("Mark 3");
		// Use window delegate
		window.SocketIOUnityEmit(Pointer_stringify(e), Pointer_stringify(data));
		
		// Free pointer
        // _free(e);
        // _free(data);
	},

	SocketIO_RequestProperty: function(keyPointer) {
		const key = Pointer_stringify(keyPointer);
		
		var value = window.socket[key] || '';
		var bufferSize = lengthBytesUTF8(value) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(value, buffer, bufferSize);
		
		return buffer;
	}

});
