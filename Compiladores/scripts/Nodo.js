class Nodo {
	constructor(simbolo = "none",nodoDer = null,nodoDown = null,terminal = null){
		this.simbolo = simbolo;
		this.nodoDer = nodoDer;
		this.nodoDown = nodoDown;
		this.terminal = terminal;
	}

	Imprimir(){
		console.log("Nodo{simbolo: "+this.simbolo+" nodoUp: "+this.nodoDer+" nodoDown: "+nodoDown);
	}

	get simbolo(){
		return this._simbolo;
	}

	set simbolo(simbolo){
		this._simbolo = simbolo;
	}

	get nodoDer(){
		return this._nodoDer;
	}

	set nodoDer(nodoDer){
		this._nodoDer = nodoDer;
	}

	get nodoDown(){
		return this._nodoDown;
	}

	set nodoDown(nodoDown){
		this._nodoDown = nodoDown;
	}

	get terminal(){
		return this._terminal;
	}

	set terminal(terminal){
		this._terminal = terminal;
	}
}