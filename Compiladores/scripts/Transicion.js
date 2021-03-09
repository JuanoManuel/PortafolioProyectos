class Transicion {

	constructor(idTransicion,valorMin,valorMax,idSalida){
        this.idTransicion = idTransicion;
        this.idSalida = idSalida;
        this.valorMax = valorMax;
        this.valorMin = valorMin;
    }

    get idTransicion(){
    	return this._idTransicion;
    }

    set idTransicion(id){
    	this._idTransicion = id;
    }

    get car(){
    	return this._car;
    }

    set car(car){
    	this._car = car;
    }

    get idSalida(){
    	return this._idSalida;
    }

    set idSalida(idSalida){
    	this._idSalida = idSalida;
    }

    get valorMax(){
        return this._valorMax;
    }

    set valorMax(valorMax){
        this._valorMax = valorMax;
    }

    get valorMin(){
        return this._valorMin;
    }

    set valorMin(valorMin){
        this._valorMin = valorMin;
    }

    Imprimir(){
    	return "[Id: "+this.idTransicion+" Car: "+this.car+" Salida: "+this.idSalida+"]";
    }
}