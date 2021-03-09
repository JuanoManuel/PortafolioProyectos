function GetMax(arreglo){
	var max = 0;
	for(var i=0;i<arreglo.length;i++)
		if(arreglo[i]>max)
			max = arreglo[i];
	return max;
}

class ListaDoble{
	constructor(){
		this.nodoInicial = null;;
		this.nodoFinalDerecho = null;
		this.nodoFinalAbajo = null;
		this.profAbajo = [];
		this.nodos = [];
		this.simbolos = [];
		this.filas = 0;
		this.profundidadDerecha = 0;
		this.profundidadAbajo = 0;
	}

	calcularProfundidadesAbajo(){
		var profundidad = 0;
		var n = this.nodoInicial;
		var n2;
		var aux = new Nodo();
		var aux2 = new Nodo();
		while(n!=null){
			n2 = n.nodoDer;
			while(n2.nodoAbajo!=null){
				profundidad++;
				aux2 = n2.nodoAbajo;
				n2 = aux2;
			}
			this.profAbajo.push(profundidad);
			this.profundidadAbajo += profundidad;
			profundidad = 0;
			aux = n.nodoAbajo;
			n = aux;
		}
	}

	calcularDatos(){
		this.filas = 0;
		var profundidadDer = [];
		var profundidad = 1;
		var n = this.nodoInicial;
		var n2;
		var aux = new Nodo();
		var aux2 = new Nodo();
		while(n !=null){
			this.filas++;
			n2 = n;
			while(n2.nodoDer != null){
				profundidad++;
				aux2 = n2.nodoDer;
				n2 = aux2;
			}
			profundidadDer.push(profundidad);
			profundidad = 1;
			aux = n.nodoAbajo;
			n = aux;
		}
		profundidadDerecha = GetMax(profundidadDer);
	}

	insertarDerecho(lista){
		if(this.nodoFinalDerecho == null && this.nodoFinalAbajo == null){
			this.nodoFinalDerecho = lista.nodoFinalDerecho;
			this.nodoFinalAbajo = lista.nodoFinalAbajo;
			this.nodoInicial = lista.nodoInicial;
			for(var i=0;i<lista.nodos.length;i++)
				this.nodos.push(lista.nodos[i]);
		} else {
			this.nodoFinalDerecho.nodoDer = lista.nodoInicial;
			this.nodoFinalDerecho = lista.nodoFinalDerecho;
			for(var i=0;i<lista.nodos.length;i++)
				this.nodos.push(lista.nodos[i]);
			this.nodoFinalDerecho.nodoDer = null;
		}
	}

	insertarAbajo(lista){
		if(this.nodoFinalDerecho == null && this.nodoFinalAbajo == null){
			this.nodoFinalDerecho = lista.nodoFinalDerecho;
			this.nodoFinalAbajo = lista.nodoFinalAbajo;
			this.nodoInicial = lista.nodoInicial;
			for(var i=0;i<lista.nodos.length;i++)
				this.nodos.push(lista.nodos[i]);
		} else {
			this.nodoFinalAbajo.nodoDown = lista.nodoInicial;
			this.nodoFinalAbajo = lista.nodoInicial;
			this.nodoFinalDerecho = lista.nodoFinalDerecho;
			for(var i=0;i<lista.nodos.length;i++)
				this.nodos.push(lista.nodos[i]);
		}
	}

	get nodoInicial(){
		return this._nodoInicial;
	}

	set nodoInicial(nodoInicial){
		this._nodoInicial = nodoInicial;
	}

	get nodoFinalDerecho(){
		return this._nodoFinalDerecho;
	}

	set nodoFinalDerecho(nodoFinalDerecho){
		this._nodoFinalDerecho = nodoFinalDerecho;
	}

	get nodoFinalAbajo(){
		return this._nodoFinalAbajo;
	}

	set nodoFinalAbajo(nodoFinalAbajo){
		this._nodoFinalAbajo = nodoFinalAbajo;
	}

	get profAbajo(){
		return this._profAbajo;
	}

	set profAbajo(profAbajo){
		this._profAbajo = profAbajo;
	}

	get nodos(){
		return this._nodos;
	}

	set nodos(nodos){
		this._nodos = nodos;
	}

	get simbolos(){
		return this._simbolos;
	}

	set simbolos(simbolos){
		this._simbolos = simbolos;
	}

	get filas(){
		return this._filas;
	}

	set filas(filas){
		this._filas = filas;
	}

	get profArriba(){
		return this._profArriba;
	}

	set profArriba(profArriba){
		this._profArriba = profArriba;
	}

	get profAbajo(){
		return this._profAbajo;
	}

	set profAbajo(profAbajo){
		this._profAbajo = profAbajo;
	}

	SetNodoInicial(lista,nodo){
		lista.nodoInicial = nodo;
		lista.nodoFinalAbajo = nodo;
		lista.nodoFinalDerecho = nodo;
		lista.nodos.push(nodo);
	}

	ImprimirLista(){
		var nodo = this.nodoInicial;
		var nodo2,nodo3,nodo4;
		var line = "i:";
		console.log("Imprimiendo...");
		while(nodo!=null){
			line += "["+nodo.simbolo+"|"+nodo.terminal+"]->";
			nodo2 = nodo;
			nodo = nodo.nodoDown;
			while(nodo2.nodoDer!=null){
				nodo2 = nodo2.nodoDer;
				if(nodo2.nodoDown!=null)
					nodo3 = nodo2.nodoDown;
				line += "["+nodo2.simbolo+"|"+nodo2.terminal+"]->";
			}
			console.log(line);
			var line2 = "         ||->"
			while(nodo3!=null){
				line2 +=  "["+nodo3.simbolo+"|"+nodo3.terminal+"]->";
				nodo4 = nodo3;
				while(nodo4.nodoDer!=null){
					nodo4 = nodo4.nodoDer;
					line2+= "["+nodo4.simbolo+"|"+nodo4.terminal+"]->";
				}
				nodo3 = nodo3.nodoDown;
				console.log(line2);
				line2 = "         ||->"
			}
			line = "|->"
		}
	}
}