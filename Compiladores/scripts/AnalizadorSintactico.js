var contenido;//gramaticas guardadas en el archivo
var listaReglas; //lista de lista dodne estan las reglas
var contador = 0; //contador para el deceso recursivo
var indice = 0;//indice usado en el decenso recursivo
var VT = [];
var VN = [];
var lex;
var lexemas = [];
var tokens = [];
var lexemaActual;
var textoTabla;
$(document).ready(function(){
	if (window.File && window.FileReader && window.FileList && window.Blob) {
	  // Great success! All the File APIs are supported.
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}
	$("#inputTablaAFD").on("change",function(){
		var fr = new FileReader();
		fr.onload = function() {
			/*AQUI HACER USO DE LA GRAMATICA*/
			textoTabla = this.result;
			console.log(textoTabla);
		}
		fr.readAsText(this.files[0]);
	});
	$("#inputGramatica").on("change",function(){
		var fr = new FileReader();
		fr.onload = function() {
			/*AQUI HACER USO DE LA GRAMATICA*/
			contenido = this.result;
			console.log(contenido);
			CreateGrammarList(contenido);
		}
		fr.readAsText(this.files[0]);
	});
});

function CreateGrammarList(file){
	//console.log(textoTabla);
	listaReglas = new ListaDoble();
	contador = 0;
	indice = 0;
	VN = GetNoTerminales(file);
	console.log("Simbolos no terminales:");
	console.log(VN);
	VT = GetTerminales(file);
	console.log("Simbolos terminales:");
	console.log(VT);
	//obtenemos arreglo de gramatica sustituido por sus tokens
	var rows = [];
	rows = textoTabla.split('♫');
	for(var i=0;i<rows.length;i++)
		rows[i] = rows[i].split('&');
	console.log(rows);
	lex = EvaluarCadena(rows,file+"a");
	while(contenido.includes("->"))
		contenido = contenido.replace("->",">");
	console.log(contenido);
	console.log("tokens de las gramaticas");
	console.log(lex);
	console.log(listaReglas);
	//una vez generados los tokens modificamso file remplazando -> por > para evitar bugs
	//a la hora de acceder los lexemas equivalentes
	//inicia decenso recursivo para la creacion de las listas
	if(G(listaReglas)){
		var t = lex.shift();
		console.log("t: "+t);
		if(t == 30){//30 corresponde al token de $ que representa fin de cadena
			listaReglas.ImprimirLista();
			//alert("Lista de gramaticas creada con exito");
			//return true;
		}
	}
	console.log(Follow("S",listaReglas));
	//return false;
}

function Follow(simbol,reglas){
	var nodo = reglas.nodoInicial;//recorre lados izquierdos
	var nodo2; //recorre lados derecos
	var nodoaux = null;//nodoaux para recorres lados izquerdos extras
	var nodoAnterior = null;
	var aux;
	var primerNT = nodo.simbolo;//primer no terminal
	var res =[];//follow
	var i;//posicion del epsilon en el first
	//si el follow es del simbolo del nodo inicial agregamos $
	if(nodo.simbolo == simbol){
		res.push("$");
	}
	while(nodo!=null){
		nodoAnterior = null;
		nodo2 = nodo.nodoDer;
		while(nodo2!=null){
			//si hay otro lado derecho para el mismo lado izq
			//lo guardamos en aux
			if(nodo2.nodoDown!=null)
				nodoaux = nodo2.nodoDown;
			//si el lado der es igual al sombolo
			if(nodo2.simbolo == simbol){
				//checamos si la regla es del tipo aBb o aB preguntando su hay un nodo derecho
				if(nodo2.nodoDer!=null && nodoAnterior!=null){
					//es una regla de la forma aBb
					//si el nodo derecho es terminal lo agregamos
					if(nodo2.nodoDer.terminal){
						res.push(nodo2.nodoDer.simbolo);
					}else{
						//si no entonces hacemos el first
						//obtenemos el first del simbolo siguiente
						aux = First(nodo2.nodoDer.simbolo);
						//buscamos la posicion donde esta el epsilon
						i = aux.indexOf("Ω");
						//si incluye epsilon le concatenamos follow del lado izq
						//y quitamos el epsilon
						if(i>=0)
							//ahora eliminamos el elemento de aux que este en la posicion i
							aux.splice(i,1);
						//unimos el follow del lado izq
						aux = Union(aux,Follow(nodo.simbolo,reglas));
						res = Union(res,aux);
					}
				}else if(nodoAnterior!=null){
					//es una regla de la forma aB
					//buscamos el follow del lado izq solo si no es el mismo lado izq
					//del que estamos buscando el follow
					if(nodo.simbolo!=simbol){
						res = Union(res,Follow(nodo.simbolo,reglas));
					}
				}
			}
			//si hay un nodo der lo asignamos a nodo2
			if(nodo2.nodoDer!=null){
				nodoAnterior = nodo2;
				nodo2 = nodo2.nodoDer;
			}else{
				//si no entonces le asignamos el nodoaux y volvemos null nodoaux
				nodoAnterior = null;
				nodo2 = nodoaux;
				nodoaux = null;
			}
		}
		nodo = nodo.nodoDown;
	}
	return res;

}

//funcion que agrega al conjunto ar1 todos los elementos de ar2 sin repetirse
function Union(ar1,ar2){
	for(var i=0;i<ar2.length;i++){
		if(!ar1.includes(ar2[i])){
			ar1.push(ar2[i]);
		}
	}
	return ar1;
}

function First(omega){
	//primero buscamos el lado izquierdo que coincida con el simbolo
	var nodo = listaReglas.nodoInicial;
	var nodofirst;
	var res = [];
	while(nodo!=null){
		if(nodo.simbolo == omega){
			//una vez encontrado checamos si el primer elemento de sus lados izquierdos
			//es terminal
			nodofirst = nodo.nodoDer;
			while(nodofirst!=null){
				if(nodofirst.terminal){
					//si es terminal lo agregamos al conjunto
					res.push(nodofirst.simbolo);
				}else{
					//si no es terminal se tiene que hacer el first de ese nodo
					res = res.concat(First(nodofirst.simbolo));
				}
				nodofirst = nodofirst.nodoDown;
			}
			return res;
		}
		nodo = nodo.nodoDown;
	}
}

/********************************/
/*funciones de decenso recursivo*/
/********************************/

function G(lista){
	console.log("G");
	if(ListaReglas(lista)){
		//console.log("regreso true");
		return true;
	}
	//console.log("regreso false");
	return false;
}

function ListaReglas(lista){
	var l2 = new ListaDoble();
	//console.log("lista reglas");
	var t;
	if(Regla(lista)){
		//console.log("Regla true desde ListaReglas:");
		//l2.ImprimirLista();
		t = lex.shift();
		//console.log("t: "+t);
		if(t==15){/*15 es el tocken del ;*/
			lexemaActual = contenido[indice];
			lexemas.push(lexemaActual);
			tokens.push(t);
			//console.log(lexemaActual+" es simbolo");
			indice++;
			if(ListaReglasP(l2)){
				//console.log("ListaReglasP true con lista:");
				////lista.ImprimirLista();
				//console.log("Lista l2");
				//l2.ImprimirLista();
				lista.insertarAbajo(l2);
				////lista.ImprimirLista();
				return true;
			}
		}else if(t==30){
			lex.unshift(t);
			return true;
		}
	}
	console.log("regreso false");
	return false;
}

function ListaReglasP(lista){
	var l2 = new ListaDoble();
	//console.log("ListaReglasP");
	var t;
	if(Regla(lista)){
		//console.log("Regla true desde ListaReglasP:");
		//l2.ImprimirLista();
		t = lex.shift();
		//console.log("t: "+t);
		if(t == 15){/*15 es el tocken del punto y coma*/
			lexemaActual = contenido[indice];
			lexemas.push(lexemaActual);
			tokens.push(t);
			//console.log(lexemaActual + " es un punto y coma");
			indice++;
			if(ListaReglasP(l2)){
				//console.log("ListaReglasP true desde ListaReglasP:");
				//l2.ImprimirLista();
				if(l2.nodoInicial != null){
					//console.log("insertando l2 a lista");
					////lista.ImprimirLista();
					lista.insertarAbajo(l2);
					//console.log("Ya agregado:")
					//lista.ImprimirLista();
				}
				//console.log("regreso true");
				return true;
			}
		}else if(t == 30){
			//console.log("Encontro fin de cadena");
			if(l2.nodoInicial!=null){
				//console.log("insetando l2:");
				//l2.ImprimirLista();
				//console.log("a: ");
				//lista.ImprimirLista();
				lista.insertarAbajo(l2);
				//console.log("Resultado:");
				//lista.ImprimirLista();
			}
			//console.log("regreso true t:"+t);
			lex.unshift(t);
			return true;
		}
		//console.log("regreso false t:"+t);
		return false;
	}
	lista = null;
	//console.log("regreso true");
	return true;
}

function Regla(lista){
	var l2 = new ListaDoble();
	//console.log("Regla");
	var t;
	if(LadoIzq(lista)){
		//console.log("LadoIzq true desde Regla: ");
		//lista.ImprimirLista();
		t = lex.shift();
		//console.log("t: "+t);
		lexemaActual = contenido[indice];
		indice++;
		if(t == 10){//10 es el tocken para la flecha
			lexemas.push(lexemaActual);
			//console.log(lexemaActual + "es una flecha");
			tokens.push(t);
			if(ListaLadosDer(l2)){
				//console.log("ListaLadosDer true desde Regla");
				//l2.ImprimirLista();
				if(l2.nodoInicial != null){
					//console.log("agregando l2");
					//l2.ImprimirLista();
					//console.log("a: ");
					//lista.ImprimirLista();
					lista.insertarDerecho(l2);
					//console.log("Resultado:")
					//lista.ImprimirLista();
				}
				//console.log("regreso true");
				
				return true;
			}
		}

	}
	//console.log("regreso false");
	lex.unshift(t);
	return false;
}

function ListaLadosDer(lista){
	//console.log("Lista lados derechos");
	var l2 = new ListaDoble();
	if(LadoDerecho(lista)){
		//console.log("LadoDerecho true desde ListaLadosDer");
		//lista.ImprimirLista();
		if(ListaLadosDerP(l2)){
			//console.log("ListaLadosDerP true desde ListaLadosDer");
			//l2.ImprimirLista();
			if(l2.nodoInicial != null){
				//console.log("Insertando l2");
				//l2.ImprimirLista();
				//console.log("a:");
				//lista.ImprimirLista();
				lista.insertarAbajo(l2);
				//lista.ImprimirLista();
			}
			//console.log("regreso true");
			return true;
		}
	}
	//console.log("regreso false");
	return false;
}

function ListaLadosDerP(lista){
	//console.log("Lista lados der p");
	var l2 = new ListaDoble();
	var t;
	t = lex.shift();
	//console.log("t: "+t);
	if(t == 20){//20 es el tocken para el OR |
		lexemaActual = contenido[indice];
		lexemas.push(lexemaActual);
		tokens.push(t);
		indice++;
		//console.log(lexemaActual+" es un or");
		if(LadoDerecho(l2)){
			//console.log("LadoDerecho true desde ListaLadosDerP:");
			//l2.ImprimirLista();
			if(l2.nodoInicial!=null){
				//console.log("agregando l2");
				//l2.ImprimirLista();
				//console.log("a: ");
				//lista.ImprimirLista();
				lista.insertarAbajo(l2);
				//console.log("Resultado");
				//lista.ImprimirLista();
			}
			if(ListaLadosDerP(lista)){
				//console.log("ListaLadosDerP true desde ListaLadosDerP");
				//console.log("regreso true");
				return true;
			}
		}
		//console.log("regreso false");
		lex.unshift(t);
		return false;
	}
	lex.unshift(t);
	//console.log("regreso true");
	return true;
}

function LadoDerecho(lista){
	//console.log("LadoDerecho");
	var t;
	var simbolos  = [];
	var l2 = new ListaDoble();
	var n;
	t = lex.shift();
	//console.log("t: "+t);
	if(t == 5){
		lexemaActual = contenido[indice];
		//console.log(lexemaActual + " es un simbolo");
		indice++;
		if(VT.includes(""+lexemaActual)){
			//console.log(lexemaActual + "es un terminal");
			n = new Nodo(""+lexemaActual);
			lexemas.push(lexemaActual);
			tokens.push(t);
			SetNodoInicial(lista,n);
			//console.log("se creo nodo inicial");
			n.terminal = true;
			//lista.ImprimirLista();
		} else if(VN.includes(""+lexemaActual)){
			n = new Nodo(""+lexemaActual);
			lexemas.push(lexemaActual);
			tokens.push(t);
			SetNodoInicial(lista,n);
			n.terminal = false;
			//console.log("Se creo nodo inicial");
			//lista.ImprimirLista();
		}
		if(LadoDerechoP(l2)){
			//console.log("LadoDerechoP true desde LadoDerecho");
			//l2.ImprimirLista();
			if(l2.nodoInicial != null){
				//console.log("Insertando l2");
				//l2.ImprimirLista();
				//console.log("a:");
				//lista.ImprimirLista();
				lista.insertarDerecho(l2);
				//console.log("Resultado");
				//lista.ImprimirLista();
			}
			//console.log("regreso true");
			
			return true;
		}
	} else if (t == 25){//25 es el token para epsilon(omega)
		lexemaActual = contenido[indice];
		indice++;
		n = new Nodo(""+lexemaActual);
		lexemas.push(lexemaActual);
		tokens.push(t);
		SetNodoInicial(lista,n);
		n.terminal = true;
		//console.log(lexemaActual + " es un epsilon");
		if(LadoDerechoP(l2)){
			//console.log("LadoDerechoP true desde LadoDerecho");
			//l2.ImprimirLista();
			if(l2.nodoInicial != null){
				//console.log("Insertando l2");
				//l2.ImprimirLista();
				//console.log("a:");
				//lista.ImprimirLista();
				lista.insertarDerecho(l2);
				//console.log("Resultado");
				//lista.ImprimirLista();
			}
			//console.log("regreso true");
			
			return true;
		}
	}
	//console.log("regreso false");
	return false;
}

function LadoDerechoP(lista){
	var t;
	//console.log("LadoDerechoP");
	var simbolos = [];
	var l2 = new ListaDoble();
	var n = new Nodo();
	t = lex.shift();
	//console.log("t: "+t);
	if(t == 5){//5 es el token para los simbolos
		lexemaActual = contenido[indice];
		//console.log(lexemaActual + " es un simbolo");
		indice++;
		if(VT.includes(""+lexemaActual)){
			//console.log(lexemaActual+" es terminal");
			n = new Nodo(""+lexemaActual);
			tokens.push(t);
			lexemas.push(lexemaActual);
			SetNodoInicial(l2,n);
			//console.log("se crea nodo inicial");
			n.terminal = true;
			//l2.ImprimirLista();
		}else if(VN.includes(""+lexemaActual)){
			//console.log(lexemaActual+" es no terminal");
			n = new Nodo(""+lexemaActual);
			tokens.push(t);
			lexemas.push(lexemaActual);
			SetNodoInicial(l2,n);
			n.terminal = false;
			//console.log("Creamos nodo inicial");
			//l2.ImprimirLista();
		}
		if(LadoDerechoP(l2)){
			//console.log("LadoDerechoP true desde LadoDerechoP");
			if(l2.nodoInicial != null){
				//console.log("agregando l2");
				//l2.ImprimirLista();
				//console.log("a:");
				//lista.ImprimirLista();
				lista.insertarDerecho(l2);
				//console.log("Resultado:");
				//lista.ImprimirLista();
			}
			//console.log("regreso true");
			return true;
		}
		//console.log("regreso false");
		return false;
	}else if(t == 25){
		lexemaActual = contenido[indice];
		indice++;
		n = new Nodo(""+lexemaActual);
		lexemas.push(lexemaActual);
		tokens.push(t);
		SetNodoInicial(lista,n);
		n.terminal = true;
		if(LadoDerechoP(l2)){
			//console.log("LadoDerechoP true desde LadoDerechoP");
			if(l2.nodoInicial != null){
				//console.log("agregando l2");
				//l2.ImprimirLista();
				//console.log("a:");
				//lista.ImprimirLista();
				lista.insertarDerecho(l2);
				//console.log("Resultado:");
				//lista.ImprimirLista();
			}
			//console.log("regreso true");
			return true;
		}
	}
	lex.unshift(t);
	//console.log("regreso true");
	return true;
}

function SetNodoInicial(lista,nodo){
	lista.nodoInicial = nodo;
	lista.nodoFinalAbajo = nodo;
	lista.nodoFinalDerecho = nodo;
	lista.nodos.push(nodo);
}

function LadoIzq(lista){
	var t;
	var n;
	//console.log("LadoIzq");
	t = lex.shift();
	//console.log("t: "+t);
	if(t == 5){//5 es el token para el simbolo
		lexemaActual = contenido[indice];
		lexemas.push(lexemaActual);
		tokens.push(t);
		//console.log(lexemaActual+" es un simbolo");
		n = new Nodo(""+lexemaActual);
		indice++;
		SetNodoInicial(lista,n);
		//console.log("se crea nodo inicial");
		//lista.ImprimirLista();
		//console.log("regreso true");
		return true;
	} else if(t == 25){//25 es el token para epsilon(omega)
		lexemaActual = contenido[indice];
		//console.log(lexemaActual+" es un epsilon");
		tokens.push(t);
		lex.unshift(t);
		//console.log("regreso true");
		return true;
	}
	//console.log("regreso false");
	return false;
}

////////////////////////////
/*FIN FUNCIONES RECURSIVAS*/
////////////////////////////


function GetTerminales(file){
	if(VN.length<=0){
		alert("Calcular no terminales primero");
		return null;
	}else{
		//hacemos un array de reglas separadas por ;
		var reglas  = file.split(';');
		var aux;//substring despues de la flecha
		var loc;
		for(var i =0;i<reglas.length;i++){
			for(var j =0;j<reglas[i].length;j++){
				//buscamos el final del simbolo flecha
				if(reglas[i][j] == '-' && reglas[i][j+1] == '>'){
					aux = reglas[i].substring(j+2,reglas[i].length);
					//remplamzamos los no terminales por una coma
					for(var k=0;k<VN.length;k++){
						loc = aux.indexOf(VN[k]);
						while(loc>=0){
							aux = aux.replace(VN[k],",");
							loc = aux.indexOf(VN[k]);
						}
					}
					//cada caracter que no sean las comas ni el | sera un terminal
					//si existe un caso especial "num" ese subscring sera un terminal
					//primero buscamos el num
					loc = aux.indexOf("num")
					if(loc>=0){
						//si lo encuentra lo quitamos de aux y agreamosa  VT
						VT.push("num");
						aux.replace("num","");
					}
					//agregaremos todos los simbolos que no sean , ni | ni $

					for(var k = 0;k<aux.length;k++){
						if(aux[k] !=',' && aux[k] != '|' && aux[k] != '$'){
							if(!isIn(aux[k],VT)){//si aun no se agrega agegarlo
								VT.push(aux[k]);
							}
						}
					}
					
				}
			}	

		}
	}
	return VT;
}

function GetNoTerminales(file){
	//hacemos un array de reglas separadas por ;
	var reglas  = file.split(';');
	var aux;
	for(var i =0;i<reglas.length;i++){
		/*para cada regla sus no terminales seras los
		que estan antes de la flecha*/
		for(var j = 0;j<reglas[i].length;j++){
			if(reglas[i][j] == '-' && reglas[i][j+1] == '>'){
				aux = reglas[i].substring(0,j);
				//si el simbolo aux ya esta en VN entonces no lo agregamos
				if(!isIn(aux,VN))
					VN.push(aux);
				continue;
			}
		}
	}
	return VN;
}

function isIn(simbol,lista){
	for(var i = 0;i<lista.length;i++){
		if(simbol == lista[i])
			return true;
	}
	return false;
}

function EvaluarCadena(tablaEstados,cadena){
		
		var current_state=1;
		var last_accepting_state=-1;
		var token = 0;
		var simbolo_index= -1;
		var token_place = tablaEstados[1].length-1;
		var last_cadena=-1;
		var lex = [];
		if (cadena.length==0)
			return 0;
		while (token < cadena.length) {

			for (var i = 0; i < tablaEstados[0].length; i++) {
				if (tablaEstados[0][i].length>1) {
					var n	= tablaEstados[0][i].split("-");
					//console.log(n);
					if (n[0]<=cadena[token] && cadena[token]<=n[1]) {
						simbolo_index = i;
						break;
					}
				}
				else{
				if (tablaEstados[0][i]==cadena[token])
					simbolo_index = i;
				}
			}
			//ver si hay transicion con el estado actual y el token actual

			if (tablaEstados[current_state][simbolo_index]!=-1) {
				current_state = tablaEstados[current_state][simbolo_index];
				token = token + 1;
				if (tablaEstados[current_state][token_place]!=-1) {
					last_accepting_state = current_state;
					last_cadena = token;
				}
			}
			else {
				if (last_accepting_state == -1) {
					current_state = 0;
					break;
				}
				else {
					token = last_cadena;
					//console.log(tablaEstados[last_accepting_state][token_place]);
					lex.push(tablaEstados[last_accepting_state][token_place]);
					current_state = 1;
				}
			}
		}
		return lex;
	}

	