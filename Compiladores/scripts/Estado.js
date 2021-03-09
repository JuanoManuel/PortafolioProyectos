class Estado {

	constructor(id,start,end,tocken){
		this.id = id;
		this.start = start;
		this.end = end;
		this.tocken = tocken;
		this.transiciones = [];
		this.token = -1;
	}

	Imprimir(){
		console.log("--------------Estado--------------");
        console.log("Estado(Id: "+this.id+" inicial: "+this.start+" final: "+this.end+" tocken: "+this.tocken+")");
        console.log("Transiciones: ");
        console.log(this.transiciones);
	}

	Agregar(data){
		this.transiciones.push(data);
	}

	Quitar(index){
		this.transiciones.splice(index,1);
	}

	get id(){
    	return this._id;
    }

    set id(id){
    	this._id = id;
    }
    get start(){
    	return this._start;
    }

    set start(ini){
    	this._start = ini;
    }

    get end(){
    	return this._end;
    }

    set end(fin){
    	this._end = fin;
    }

    get idTransicion(){
    	return this._idTransicion;
    }

    set idTransicion(id){
    	this._idTransicion = id;
    }
}
