var tablaR0 = [];
var historialCerraduras;
var countConjuntos;
$(document).ready(function(){
	$("#btnLR0").on("click",function(){
		console.log("Generando tabla LR0");
		LR0(listaReglas);
	});
});

function LR0(reglas){
	///////////////////////////////////////////////////////////////////////
	//se agrega la regla aumentada Q -> ladoizq(primera regla)/////////////
	var reglaA = new ListaDoble();//reglas aumentadas
	var reglaaux = new ListaDoble();
	var n = new Nodo("Q");
	SetNodoInicial(reglaA,n);
	var n2 = new Nodo(""+reglas.nodoInicial.simbolo);
	var aux;
	var index;//unbiacion del punto
	n2.terminal = false;
	SetNodoInicial(reglaaux,n2);
	reglaA.insertarDerecho(reglaaux);
	reglaA.insertarAbajo(reglas);
	console.log("Regla aumentada");
	reglaA.ImprimirLista();
	/*Termina creacion de regla aumentada*/
	//diccionario de reglas que  utlizaremos para la generacion de la tabla
	var dicReglas = CreateDictionary(reglaA);
	historialCerraduras = [];
	//////////////////////////////////////////////////
	//comenzamos con la cerradura de la primer regla//
	//////////////////////////////////////////////////
	var conjuntos = [];
	countConjuntos = 1;
	//obtenemos S0
	var item = ConvertirAItem(reglaA.nodoInicial);
	historialCerraduras.push(item);
	conjuntos.push(Cerradura(item,reglaA));
	var row;//fila de la tabla
	var alfa;//alfa a la que se le hace la operacion IrA
	//seguimos con obtener los demas conjuntos
	//recorre todos los conjuntos
	for(var i=0;i<conjuntos.length;i++){
		var tamrow = VT.length+VN.length+1;
		row = new Array(tamrow);
		//inicializamos row con puros -
		for(var k = 0;k<row.length;k++)
			row[k] = "-";
		tablaR0.push(row);
		//console.log("tablaR0");
		//ImprimirTabla(tablaR0);
		//recorre cada uno de los elementos de un conjunto
		for(var j=0;j<conjuntos[i].length;j++){
			index = conjuntos[i][j].indexOf('.');
			alfa = conjuntos[i][j][index+1];
			aux = IrA(conjuntos[i][j],alfa,reglaA,row);
			if(aux!=0){
				conjuntos.push(aux);
				countConjuntos++;
			}
			/////////////////////////////////////////////////
			//Etapa de agregacion de reducciones a la tabla//
			/////////////////////////////////////////////////
			let numRegla;//numero de regla asociada al item con punto al final
			let simbTabla;//lista de simbolos obtenidos por el follow al lado izq de la regla asociada
			//console.log(Follow(dicReglas[9][0],listaReglas));
			if(index==conjuntos[i][j].length-1){
				numRegla = FindNumRegla(dicReglas,conjuntos[i][j]);
				console.log("El conjunto "+i+" tiene una reduccion ala regla numero "+numRegla+" en el item "+conjuntos[i][j]);
				if(numRegla==0){
					console.log("Entramos al caso de la regla aumentada");
					console.log("agregando Aceptar a estado "+i+" en simbolo $");
					tablaR0[i][ObtenerColumnaTablaLR0("$")] = "A";
				}else{
					console.log("Follow de "+dicReglas[numRegla][0]);
					simbTabla = Follow(dicReglas[numRegla][0],reglas);
					console.log(simbTabla);
					for(let x=0;x<simbTabla.length;x++){
						console.log("Insertando a lista ["+i+"]["+ObtenerColumnaTablaLR0(simbTabla[x])+"]: r"+numRegla);
						tablaR0[i][ObtenerColumnaTablaLR0(simbTabla[x])] = "r"+numRegla;
					}
				}
			}
		}
	}
	


	console.log("Tabla");
	console.log(tablaR0);
	ImprimirTabla(tablaR0);
	MostrarTablaLR0(tablaR0);
	$("#modalTablaLR0").modal("show");
}

function IrA(S,alfa,reglas){
	let index;
	item = Mover(S);
	//console.log("Analizando: "+S);
	//si es -1 significa que el punto esta hasta el final y no se busca cerradura
	if(item!=-1){
		//vemos si no se a incluido esa cerradura al historial
		index = historialCerraduras.indexOf(item);
		if(index<0){
			historialCerraduras.push(item);
			if(VT.includes(alfa)){
				tablaR0[tablaR0.length-1][ObtenerColumnaTablaLR0(alfa)] = "d"+countConjuntos;
				//console.log("Insertando a lista ["+(tablaR0.length-1)+"]["+ObtenerColumnaTablaLR0(alfa)+"]: "+countConjuntos);
			}
			else{
				tablaR0[tablaR0.length-1][ObtenerColumnaTablaLR0(alfa)] = countConjuntos;
			}
			return Cerradura(item,reglas);
		}else{
			if(VT.includes(alfa)){
				tablaR0[tablaR0.length-1][ObtenerColumnaTablaLR0(alfa)] = "d"+index;
				//console.log("Insertando a lista ["+(tablaR0.length-1)+"]["+ObtenerColumnaTablaLR0(alfa)+"]: "+index);
			}
			else{
				tablaR0[tablaR0.length-1][ObtenerColumnaTablaLR0(alfa)] = index;
			}
			return 0;
		}
	}

	return 0;
}

function ImprimirTabla(tabla){
	let tam;
	//primero imprimimos los encabezados
	let row = " ";
	for(let i=0;i<VT.length;i++)
		row += VT[i];
	row += "$";
	for(let i=0;i<VN.length;i++)
		row += VN[i];
	console.log(row);
	for(let i=0;i<tabla.length;i++){
		row = i+":";
		tam = tabla[i].length
		for(let j=0;j<tam;j++){
			row += tabla[i][j];
		}
		console.log(row);
	}
}

function MostrarTablaLR0(tabla){
	//generamos xcabezeras;
	var thead = $("#tablaLR0 thead");
	var tbody = $("#tablaLR0 tbody");
	thead.append("<tr>");
	thead.append("<th></th>");
	for(let i=0;i<VT.length;i++)
		thead.append("<th style='font-size: 15px; padding: 0px;' scope='col'>"+VT[i]+"</th>");
	thead.append("<th style='font-size: 15px; padding: 0px;' scope='col'>$</th>");
	for(let i=0;i<VN.length;i++)
		thead.append("<th style='font-size: 15px; padding: 0px;' scope='col'>"+VN[i]+"</th>")
	//llenamos la tabla
	for(var i=0;i<tabla.length;i++){
		tbody.append("<tr>");
		tbody.append("<td style='font-size: 15px; padding: 0px;'>"+i+"</td>");
		for(var j=0;j<tabla[i].length;j++){
			tbody.append("<td style='font-size: 15px; padding: 0px;'>"+tabla[i][j]+"</td>");
		}
		tbody.append("</tr>");
	}
}

function FindNumRegla(diccionario,item){
	item = item.replace(".","");
	item = item.replace(">","");
	return diccionario.indexOf(item);
}

function CreateDictionary(reglas){
		let diccionario = [];
		let nodo = reglas.nodoInicial;
		let nodo2,nodo3,nodo4;
		let regla;
		while(nodo!=null){
			regla = ""+nodo.simbolo;
			nodo2 = nodo;
			while(nodo2.nodoDer!=null){
				nodo2 = nodo2.nodoDer;
				if(nodo2.nodoDown!=null)
					nodo3 = nodo2.nodoDown;
				regla += ""+nodo2.simbolo;
			}
			diccionario.push(regla);
			
			while(nodo3!=null){
				regla = ""+nodo.simbolo;
				regla +=  ""+nodo3.simbolo;
				nodo4 = nodo3;
				while(nodo4.nodoDer!=null){
					nodo4 = nodo4.nodoDer;
					regla+= ""+nodo4.simbolo;
				}
				diccionario.push(regla);
				nodo3 = nodo3.nodoDown;
			}
			nodo = nodo.nodoDown;
		}

		return diccionario;
	}

function ObtenerColumnaTablaLR0(alfa){
	var index = VT.indexOf(alfa);
	if(alfa == "$"){
		return VT.length;
	}else if(index>=0){
		return index;
	}else{
		index = VN.indexOf(alfa);
		if(index>=0){
			return VT.length+index+1;
		}
	}
}

function Cerradura(item,reglas){
	var res = [];
	res.push(item);
	var index;
	var simbol;
	//console.log(res);
	for(var i=0;i<res.length;i++){
		index = res[i].indexOf('.');
		if(index+1<res[i].length){
			simbol = res[i][index+1];

			if(VN.includes(simbol)){
				//console.log("simbolo: "+simbol);
				res = Union(res,CrearItems(simbol,reglas));
			}
			//console.log(simbol+" es terminal");
		}
	}
	return res;
}


function CrearItems(simboloIzq,reglas){
	var items = [];
	var item;
	var nodo = reglas.nodoInicial;
	var nodo2,nodoaux;
	var i=0;
	while(nodo!=null){
		if(nodo.simbolo==simboloIzq){
			item = simboloIzq+">.";
			nodo2 = nodo.nodoDer;
			while(nodo2!=null){
				item +=nodo2.simbolo;
				if(nodo2.nodoDown!=null){
					nodoaux = nodo2.nodoDown;
				}
				if(nodo2.nodoDer!=null){
					nodo2 = nodo2.nodoDer;
				}else{
					items.push(item);
					item = simboloIzq+">.";
					nodo2 = nodoaux;
					nodoaux = null;
				}
			}
			//console.log("items creados: ");
			//console.log(items);
			return items;
		}
		nodo = nodo.nodoDown;
	}
}

function ConvertirAItem(nodo){
	//console.log(nodo);
	var item=""+nodo.simbolo+">.";
	while(nodo.nodoDer!=null){
		nodo = nodo.nodoDer;
		item += nodo.simbolo;

	}
	return item;
}

function Mover(regla){
	var index = regla.indexOf('.');//posicion del punto
	var newregla = regla.substring(0,index);
	if(index>=0){
		if(index==regla.length-1){
			return -1;
		}else{
			newregla += regla[index+1]+regla[index];
			if(index+1<regla.length-1)
				newregla += regla.substring(index+2);
		}
	}else{
		alert("Mover: punto no encontrado: "+regla);
	}
	return newregla;
}