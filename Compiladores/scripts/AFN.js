var numEstados=0;//variable global cuanta estados
var numAFN = 0;//variable global cuenta afns
var numTransiciones=0;//variable global cuenta transicioens
var nodos = new vis.DataSet(); //nodos totales
var aristas = new vis.DataSet(); //aristas totales
var contenedor = document.getElementById("contenedor"); //contenedor del grafo
var datos = {
	nodes: nodos,
	edges: aristas
};
var opciones = {
	edges:{
		arrows:{
			to:{
				enabled:true
			}
		}
	}
};
var grafo = new vis.Network(contenedor,datos,opciones);
//limpia contenedor para repintar despues
function UnirTodo(afns){
	var tokens = 0;
	//Creamos AFN con caracter cualquiera
	var nuevoAFN = new AFN('-1');
	//borramos alfabeto
	nuevoAFN.alfabeto = [];
	//borramos estados
	nuevoAFN.estados = [];
	//Creamos estado inicial
	var e1 = new Estado(numEstados++,true,false,1);
	//agregamos estado inicial al nuevoAFN
	nuevoAFN.estados.push(e1);
	//por cada afn agregamos cada uno de los estados a un nuevo AFN
	//ademas vamos creando el nuevo alfabeto evitando repetciones
	for(var i=0;i<afns.length;i++){
		for(var j=0;j<afns[i].estados.length;j++){
			nuevoAFN.estados.push(afns[i].estados[j]);
		}
		for(var j=0;j<afns[i].alfabeto.length;j++){
			if(!nuevoAFN.isInAlphabet(afns[i].alfabeto[j]))
				nuevoAFN.alfabeto.push(afns[i].alfabeto[j]);
		}
	}
	//Del nuevo afn vamos a crear transiciones desde el primer estado que es "e1" a todos los demas estados
	//identificados como iniciales ademas les quitaremos dicha propiedad para que solo "e1" sea el inicial
	for(var i=1/*omitimos posicion cero por es "e1"*/;i<nuevoAFN.estados.length;i++){
		if(nuevoAFN.estados[i].start){
			nuevoAFN.estados[i].start = false;
			nuevoAFN.estados[0].transiciones.push(new Transicion(numTransiciones++,'ɛ','ɛ',nuevoAFN.estados[i].id));
		}
	}
	
	for (var i = 0; i < nuevoAFN.estados.length; i++) {
		if (nuevoAFN.estados[i].end) {
			nuevoAFN.estados[i].token = tokens+5;
			tokens +=5;
			console.log(nuevoAFN.estados[i].token);
		}
	}
	console.log(nuevoAFN);
	//retornamos nuevo automata
	return nuevoAFN;
}


function LimpiarContenedor(){
	nodos = new vis.DataSet();
	aristas = new vis.DataSet();
	datos = {
		nodes: nodos,
		edges: aristas
	};
	var grafo = new vis.Network(contenedor,datos,opciones);
}

function DibujarAFNs(afns){
	LimpiarContenedor();
	for(var i=0;i<afns.length;i++)
		DibujarAFN(afns[i]);
}

function DibujarAFN(afn){
	try{
		/*****Recorremos todos los estados************/
		for(var i=0;i<afn.estados.length;i++){
			/* por cada estado recorrido se agrega un nodo al grafo */
			var num = afn.estados[i].id;
			var name = ""+num;
			//se le asigna un color dependiendo de que tipo de estado sea
			if(afn.estados[i].start){//Dibuja estado inicial
				nodos.add({
					id:num,
					label:name,
					color: '#81F7BE'
				});
			} else if(afn.estados[i].end){//dbuja estado final
				nodos.add({
					id:num,
					label:name,
					color: '#F78181'
				});
			} else{ //dibuja otro
				nodos.add({
					id:num,
					label:name
				});
			}
			/* fin de agregacion de nodos*/
		}
		/****recorremos todos los estados*******/
		for(var i=0;i<afn.estados.length;i++){
			var idFrom = afn.estados[i].id; //id de origen de la transicion
			var caracteres; //simbolos de la transicion
			var idTo; //destino de la transicion
			/*****recorremos transiciones del estado****/
			for(var j=0;j<afn.estados[i].transiciones.length;j++){
				/***por cada transicion se agrega un edge al grafo***/
				//si valorMin y valorMax son los mismos entonces
				if(afn.estados[i].transiciones[j].valorMax==afn.estados[i].transiciones[j].valorMin)
					caracteres = afn.estados[i].transiciones[j].valorMax;
				else{ //si no
					caracteres = "("+afn.estados[i].transiciones[j].valorMin+"-"+afn.estados[i].transiciones[j].valorMax+")";
				}
				idTo = afn.estados[i].transiciones[j].idSalida;
				/**Agremos edge al grafo**/
				aristas.add([
					{from:idFrom, to:idTo, label:caracteres}
				]);
			}
		}
	} catch(err){
		alert(err);
	}
}


class AFN {
	constructor(car){
		/*Obtenemos valor max y valor min del string car*/
		var valores = car.split(",");
		var aux;
		if(valores.length>1){
			if(valores[0]>valores[1]){/*Verificamos que en la posicion 0 este el menos y en la 1 el mayor*/
				aux = valores[0];
				valores[0] = valores[1];
				valores[1] = aux;
			}
			this.alfabeto = [(valores[0]+","+valores[1])];
		}else{
			valores[1] = valores[0];
			this.alfabeto = [valores[0]];
		}
		
		this.estados = [];
		this.id = numAFN++;
		this.estados.push(new Estado(numEstados++,true,false));
		this.estados.push(new Estado(numEstados++,false,true));
		var t = new Transicion(numTransiciones++,valores[0],valores[1],this.estados[1].id);
		var ts = [t];
		this.estados[0].transiciones = ts;
	}

	Union(afn){
		var t1,t2,t3,t4;
		var result;
		/*Se genera el nuevo estado inicial*/
		var e1 = new Estado(numEstados++,true,false);
		/*Remueve propiedad del actual estado inicial//
		//y crea una nueva transicion en afn local--------*/
		for(var i=0;i<this.estados.length;i++){
			if(this.estados[i].start){
				this.estados[i].start = false;
				t1 = new Transicion(numTransiciones++,'ɛ','ɛ',this.estados[i].id);
			}
		}
		//En afn
		for(var i=0;i<afn.estados.length;i++){
			if(afn.estados[i].start){
				afn.estados[i].start = false;
				t2 = new Transicion(numTransiciones++,'ɛ','ɛ',afn.estados[i].id);
			}
		}

		var trans = [t1,t2];
		e1.transiciones = trans;
		/*********Se genera nuevo estado final*******************/
		var e2 = new Estado(numEstados++,false,true);
		/*Remueve estados finales y crea transiciones al nuevo estado final*/
		//En afn local
		for(var i=0;i<this.estados.length;i++){
			if(this.estados[i].end){
				this.estados[i].end = false;
				t3 = new Transicion(numTransiciones++,'ɛ','ɛ',e2.id);
				//Agregamos transicion al estado que era estado final
				this.estados[i].transiciones.push(t3);
			}
		}
		/*En afn*/
		for(var i=0;i<afn.estados.length;i++){
			if(afn.estados[i].end){
				afn.estados[i].end = false;
				t4 = new Transicion(numTransiciones++,'ɛ','ɛ',e2.id);
				//Agregamos transicion al estado que era estado final
				afn.estados[i].transiciones.push(t4);
			}
		}
		/* se crea nuevo alfabeto*/
		for(var i=0;i<afn.alfabeto.length;i++)
			if(!this.isInAlphabet(afn.alfabeto[i]))
				this.alfabeto.push(afn.alfabeto[i]);
		/*Se agregan estados del afn y los dos nuevos estados al array del afn local*/
		for(var i=0;i<afn.estados.length;i++)
			this.estados.push(afn.estados[i]);
		this.estados.push(e1);
		this.estados.push(e2);
		return this;
	}

	Concatenar(afn){
		var idEnd = this.findEndIndex(); //obtenemos index de estado final del afn local
		var idInicio = afn.findStartIndex();//obtenemos index de estado inicial del afn a concatenar
		/*Redidirigimos todas las transiciones del afn que tengan como idSalida el id del estado inicial del afn*/
		for(var i=0;i<afn.estados.length;i++){ //recorre estados
			for(var j=0;j<afn.estados[i].length;j++){
				if(afn.estados[i].transiciones[j].idSalida == idInicio)
					afn.estados[i].transiciones[j].idSalida = idEnd
			}
		}
		/* Las transiciones contenidas en el estado incial del afn las movemos al estado final del afn local*/
		for(var i=0;i<afn.estados[idInicio].transiciones.length;i++)
			this.estados[idEnd].transiciones.push(afn.estados[idInicio].transiciones[i]);
		//al estado final de afn local lo volvemos estado normal
		this.estados[idEnd].end = false;
		//removemos el estado inicial del afn
		afn.estados.splice(idInicio,1);
		//agregamos todos los estados de afn a afn local
		for(var i=0;i<afn.estados.length;i++){
			this.estados.push(afn.estados[i]);
		}
		//actualizamos alfabeto del nuevo AFN
		for(var i=0;i<afn.alfabeto.length;i++){
			if(!this.isInAlphabet(afn.alfabeto[i]))
				this.alfabeto.push(afn.alfabeto[i]);
		}
		//retornamos el autamata actualizado
		return this;
	}


	CerraduraPositiva(){

		var t1, t2, t3; //Transiciones nuevas
		var e; //Estado auxuliar
		//Obtener el estado inicial
		for(var i=0; i<this.estados.length;i++){
			if(this.estados[i].start)
				e = this.estados[i].id;
		}
		//Transicion epsilon del estado final al anicial
		t1 =  new Transicion(numTransiciones++,'ɛ','ɛ',e);

		//Agregar transicion al estado final
		for(var i=0; i<this.estados.length; i++){
			if(this.estados[i].end)
				this.estados[i].transiciones.push(t1);
		}
		//Crear un nuevo estado inicial
		 var ei = new Estado(numEstados++, true, false);

		 //Quitar true al otro estado inicial
		 for(var i=0; i<this.estados.length;i++){
	 		if(this.estados[i].start){
	 			this.estados[i].start = false;
				t2 = new Transicion(numTransiciones++,'ɛ','ɛ',this.estados[i].id)
			}
	 	}
		//Agregar transision a ei
		ei.transiciones.push(t2)

		//Crear un nuevo estado final
		var ef = new Estado(numEstados++,false,true);
		//Eliminar el antiguo estado final
		for (var i = 0; i < this.estados.length; i++) {
			if(this.estados[i].end){
				this.estados[i].end = false;
				//creamos la nueva transicion al estado final
				t3 = new Transicion(numTransiciones++,'ɛ','ɛ',ef.id);
				this.estados[i].transiciones.push(t3);
			}
		}

		this.estados.push(ei);
		this.estados.push(ef);

		return this;

	}

	C_Estrella(){
		var t; //transision nueva
		var e;

		this.CerraduraPositiva();
//Obtener id del estado inicial
		for(var i=0; i<this.estados.length;i++){
			if(this.estados[i].end)
				e = this.estados[i].id;
		}

		//Transicion epsilon del estado final al anicial
		t	 =  new Transicion(numTransiciones++,'ɛ','ɛ',e);

		//Agregar transicion al estado final
		for(var i=0; i<this.estados.length; i++){
			if(this.estados[i].start)
				this.estados[i].transiciones.push(t);
		}

		return this;

	}
	
	Opcional(){
		var t1, t2, t3;
		var id_start, id_end;
		var new_start, new_end;
		//Encontrar el estado inicial y quitarle que sea inicial cx
		for (var i = 0; i < this.estados.length; i++) {
			if (this.estados[i].start){
				id_start = this.estados[i].id;
				this.estados[i].start=false;
			}
		}
		//Creamos la una nueva transicion
		t1 = new Transicion(numTransiciones++,'ɛ','ɛ',id_start);
		//Creamos el nuevo estado inicial
		new_start = new Estado(numEstados++,true,false);
		new_start.transiciones.push(t1);

		//Creamos el nuevo estado finales
		new_end = new Estado(numEstados++,false,true);
		id_end = new_end.id;
		//Creamos la nueva transicion para el estado finales
		t2 = new Transicion(numTransiciones,'ɛ','ɛ',id_end);

		for (var i = 0; i < this.estados.length; i++) {
			if (this.estados[i].end){
				this.estados[i].transiciones.push(t2);
				this.estados[i].end = false;
			}
		}
		//Crear la transicion que va del estado inicual al final
		t3 = new Transicion(numTransiciones++,'ɛ','ɛ',id_end);
		new_start.transiciones.push(t3);
		this.estados.push(new_start);
		this.estados.push(new_end);

		return this;
	}

	
	cerradura_e(estado){
		var stack_estado=[];
		var conjuntoC=[];
		var s;
		stack_estado.push(estado);
		while (stack_estado.length!=0) {
			estado=stack_estado.pop();
			if (conjuntoC.find(function(element) {
				element=estado;
			  }))
			  continue;
			conjuntoC.push(estado);
			estado.transiciones.forEach(element => {
				for(s=element.valorMin;s<=element.valorMin;s++){
				if (s=="ɛ") {
					stack_estado.push(this.findEstado(element.idSalida));
				}}
			});
		}
		return conjuntoC;
    }
    
    mover_e(estado ,caracter ){
		var conjuntoR=[];
		var estadoAux;
		estado.transiciones.forEach(element => {
			if(caracter >=element.valorMin  && caracter<=element.valorMax){
				estadoAux=this.findEstado(element.idSalida);
				conjuntoR.push(estadoAux);
			}
		});
		return conjuntoR;
	}
	mover(conjuntoS, caracter){
		var conjuntoR= [];
		var conjuntoAux= [];
		conjuntoS.forEach(element => {
			conjuntoAux=this.mover_e(element,caracter);
			if(conjuntoAux.length!=0)
			conjuntoR=conjuntoR.concat(conjuntoAux);
		});
		return conjuntoR;
	}

	
	ir_a(conjuntoS,caracter){
		var conjuntoR= [];
		var conjuntoIR= [];
		var conjuntoaux= [];
		conjuntoR=this.mover(conjuntoS,caracter);	
		if(conjuntoR.length!=0){
		conjuntoR.forEach(element => {
			conjuntoaux=this.cerradura_e(element);
			if(conjuntoaux.length!=0)
			conjuntoIR=conjuntoIR.concat(conjuntoaux);
		});}
		return conjuntoIR;
	}
	findEstado(idS){
		for(var i=0;i<this.estados.length;i++){
			if(this.estados[i].id==idS)
				break;
				
		}
		return this.estados[i];
	}

	searchFinal(){
		var conjuntoF= [];
		for(var i=0;i<this.estados.length;i++){
			if(this.estados[i].end)
				conjuntoF.push(this.estados[i].id);
		}
		console.log(conjuntoF.sort());
		return conjuntoF;
		
	}

	
	/*retorna el index del array de estados en el que se encuentra el estado final*/
	findEndIndex(){
		for(var i=0;i<this.estados.length;i++){
			if(this.estados[i].end)
				return i;
		}
		return -1;
	}
	/*retorna el index del array de estados en el que se encuentra el estado inicial*/
	findStartIndex(){
		for(var i=0;i<this.estados.length;i++){
			if(this.estados[i].start)
				return i;
		}
		return -1;
	}
	/*retorna true si el simbolo esta en el alfabeto*/
	isInAlphabet(simbolo){
		for(var i=0;i<this.alfabeto.length;i++)
			if(this.alfabeto[i]==simbolo)
				return true;
		return false;
	}

}
