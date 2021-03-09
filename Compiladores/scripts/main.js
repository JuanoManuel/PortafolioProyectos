var automatas = []; //array de automatas creados
var claseBoton; //variable que contiene el nombre de la clase agreada al boton del modal
var tabla;//Tabla del afd
//var textoTabla = "a-z&A-Z&0-9&'&-&>&;&|&Ω&$♫2&3&-1&-1&4&-1&5&6&7&8&-1♫-1&-1&9&10&-1&-1&-1&-1&-1&-1&5♫-1&-1&9&10&-1&-1&-1&-1&-1&-1&5♫-1&-1&-1&-1&-1&11&-1&-1&-1&-1&-1♫-1&-1&-1&-1&-1&-1&-1&-1&-1&-1&15♫-1&-1&-1&-1&-1&-1&-1&-1&-1&-1&20♫-1&-1&-1&-1&-1&-1&-1&-1&-1&-1&25♫-1&-1&-1&-1&-1&-1&-1&-1&-1&-1&30♫-1&-1&9&-1&-1&-1&-1&-1&-1&-1&5♫-1&-1&-1&-1&-1&-1&-1&-1&-1&-1&5♫-1&-1&-1&-1&-1&-1&-1&-1&-1&-1&10♫";
$(document).ready(function(){

	/*Funcion que se manda a llamar para geerar el automata*/
	$("#btnGenerarAutomata").on("click",function(){
		var pila = []; //pila donde se guardaran los simbolos de la expresion regular
		var er = $("#inputRegla").val(); //expresion regular
		/*LLenamos la pila*/
		console.log(er);
		for(var i=er.length-1;i>=0;i--){
			pila.push(er.charAt(i));
		}
		var final = GenerarAutomata(pila);
		console.log(final);
		automatas.push(final);
		DibujarAFNs(automatas);
	});

	//Funcion que se manda a llama cuando se presiona EvaluarCadena
	$("#btnEvaluarCadena").on("click",function() {
		console.log("Entrando a evaluar cadena");
		//se crea la tabla haciendo uso del string guardado
		var rows = [];
		rows = textoTabla.split('♫');
		for(var i=0;i<rows.length;i++)
			rows[i] = rows[i].split('&');
		console.log(rows);
		var lexema = EvaluarCadena(rows);
		console.log(lexema);
	});

	function download(data, filename, type) {
	    var file = new Blob([data], {type: type});
	    if (window.navigator.msSaveOrOpenBlob) // IE10+
	        window.navigator.msSaveOrOpenBlob(file, filename);
	    else { // Others
	        var a = document.createElement("a"),
	                url = URL.createObjectURL(file);
	        a.href = url;
	        a.download = filename;
	        document.body.appendChild(a);
	        a.click();
	        setTimeout(function() {
	            document.body.removeChild(a);
	            window.URL.revokeObjectURL(url);
	        }, 0); 
	    }
	}

	/*Funciones que se mandan a llamar cuando se da click a una operacion AFN
	*Descripcion: en el html hay una plantilla de modal con el id templateModal, con
	*con las siguientes funciones se modificara el contenido de la plantilla dependiendp
	*de la operacion solicitada*/

	/*Al dar click en nuevo AFN*/
	$("#btnNuevo").on("click",function(){ //Agrega los automatas creados a las combobox
		/*codigo html que se mostrara como cuerpo del modal*/
		var contenido = "<div class='custom-control custom-radio custom-control-inline'>"
			+"<input type='radio' id='inputRadio1' name='inputRadio' class='custom-control-input'><label class='custom-control-label' "
			+"for='inputRadio1'>Un simbolo</label></div><div class='custom-control custom-radio custom-control-inline'>"
			+"<input type='radio' id='inputRadio2' name='inputRadio' class='custom-control-input'><label class='custom-control-label' "
			+"for='inputRadio2'>Conjunto de simbolos</label></div>"
			+"<div class='input-group mt-3'><div class='input-group-prepend'>"
			+"<span class='input-group-text'>Valor minimo:</span></div>"
			+"<input type='text' class='form-control' id='inputValorMin'><div class='input-group-prepend'>"
			+"<span class='input-group-text'>Valor maximo:</span></div>"
			+"<input type='text' class='form-control' id='inputValorMax'></div>";
		claseBoton = "nuevoAFN";
		generarModal("Crear nuevo AFN",contenido,"Crear");//funcion para generar modal
		$("#templateModal").modal("show");//mostramos modal
	});

	/*Al dar click en union*/
	$("#btnUnion").on("click",function(){
		//codigo html que se mostrara como cuerpo en el modal
		var contenido = "<div class='form-group'><div class='input-group mb-3'>"+
            "<div class='input-group-prepend'><label class='input-group-text' "
            +"for='inputGroupSelect01'>Automatas</label></div>"
            +"<select class='custom-select' id='inputGroupSelect01'><option selected>"
            +"1er AFN</option></select><select class='custom-select' id='inputGroupSelect02'>"
            +"<option selected>2do AFN</option></select></div></div>";
        claseBoton = "unirAFN";
        generarModal("Unir dos AFN",contenido,"Unir");
        $("#btnUnirTodo").fadeIn();
        $("#templateModal").modal("show");//mostramos modal
        //Agrega los automatas creados a los combobox
		for(var i=0;i<automatas.length;i++){
			$("#inputGroupSelect01").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
			$("#inputGroupSelect02").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
		}
	});

	//Al dar click a unir todo
	$("#btnUnirTodo").on("click",function(){
		console.log("entro");
		var nuevo = UnirTodo(automatas);
		automatas = [];
		automatas.push(nuevo);
		DibujarAFNs(automatas);
		$("#btnUnirTodo").fadeOut();
		$("#templateModal").modal("hide");//escondemos modal
		$("#btnSubmit").removeClass("unirAFN");

	});

	/*Al dar click en concatenar*/
	$("#btnConcatenar").on("click",function(){
		var contenido = "<div class='form-group'><div class='input-group mb-3'>"+
            "<div class='input-group-prepend'><label class='input-group-text' "
            +"for='inputGroupSelect01'>Automatas</label></div>"
            +"<select class='custom-select' id='inputGroupSelect01'><option selected>"
            +"1er AFN</option></select><select class='custom-select' id='inputGroupSelect02'>"
            +"<option selected>2do AFN</option></select></div></div>";
        claseBoton = "concatenarAFN";
        generarModal("Concatenar dos AFN",contenido,"Concatenar");
        $("#templateModal").modal("show");//mostramos modal
        //Agrega los automatas creados a los combobox
		for(var i=0;i<automatas.length;i++){
			$("#inputGroupSelect01").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
			$("#inputGroupSelect02").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
		}
	});
	/*Al dar click en cerradura positiva*/
	$("#btnCerraduraPositiva").on("click",function(){
		var contenido = "<div class='form-group'><div class='input-group mb-3'>"+
            "<div class='input-group-prepend'><label class='input-group-text' "
            +"for='inputGroupSelect01'>Automatas</label></div>"
            +"<select class='custom-select' id='inputGroupSelect01'><option selected>"
            +"1er AFN</option></select></div></div>";
        claseBoton = "positivaAFN";
        generarModal("Crear cerradura positiva",contenido,"Crear");
        $("#templateModal").modal("show");
        for(var i=0;i<automatas.length;i++){
			$("#inputGroupSelect01").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
			$("#inputGroupSelect02").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
		}
	});
	/*Al dar click en cerradura de kleene*/
	$("#btnCerraduraKleene").on("click",function(){
		var contenido = "<div class='form-group'><div class='input-group mb-3'>"+
            "<div class='input-group-prepend'><label class='input-group-text' "
            +"for='inputGroupSelect01'>Automatas</label></div>"
            +"<select class='custom-select' id='inputGroupSelect01'><option selected>"
            +"1er AFN</option></select></div></div>";
        claseBoton = "kleeneAFN";
        generarModal("Crear cerradura de kleene",contenido,"Crear");
        $("#templateModal").modal("show");
        for(var i=0;i<automatas.length;i++){
			$("#inputGroupSelect01").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
			$("#inputGroupSelect02").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
		}
	});
	/*Al dar click en generar AFD*/
	$("#btnAFD").on("click",function(){
		console.log(automatas[0]);
		var afd = new AFD('d');
		tabla = afd.convertir_AFD(automatas[0]);
		console.log(tabla);
		MostrarTablaAFD(tabla);
		//creamos un string equivalente a tabla donde cada celda se separa por "comas"
		//y cada fila es un ♫
		var texto = "";
		for(var i=0;i<tabla.length;i++){
			for(var j=0;j<tabla[i].length;j++){
				if(j+1>=tabla[i].length)
					texto += tabla[i][j] +"♫";
				else
					texto += tabla[i][j] + "&";
			}
		}
		console.log(texto);
		//bake_cookie("tablaAFD",texto);
		//guardamos la tabla AFD en un txt
		download(texto,"tablaAFD.txt","text/plain;charset=utf-8");
		$("#modalTablaAFD").modal("show");
	});
	/*Al dar click en opcional*/
	$("#btnOpcional").on("click",function(){
		var contenido = "<div class='form-group'><div class='input-group mb-3'>"+
            "<div class='input-group-prepend'><label class='input-group-text' "
            +"for='inputGroupSelect01'>Automatas</label></div>"
            +"<select class='custom-select' id='inputGroupSelect01'><option selected>"
            +"1er AFN</option></select></div></div>";
        claseBoton = "opcionalAFN";
        generarModal("Operacion opcional",contenido,"Crear");
        $("#templateModal").modal("show");
        for(var i=0;i<automatas.length;i++){
			$("#inputGroupSelect01").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
			$("#inputGroupSelect02").append("<option value='"+i+"'>Id: "+i+" alfabeto: "+automatas[i].alfabeto+"</option>");
		}
	});
	/******************FIN FUNCIONES OPERACIONES AFN*************************/

	/**********Funciones para generar AFNs ********************************/
	/*Descripcion: al generarse el modal el boton de modal tiene agregada una clase
	**Ej. si se clickeo en nuevo AFN el boton del modal tendra una clase nuevoAFN
	**Dependiendo de la clase agregada es la funcion se que ejecutara
	*/

	$("#btnSubmit").on("click",function(){
		//al dar click en el boton se determinara cual es la clase que tiene y sabiendo esto
		//se ejecutaran las instrucciones correspondientes
		if($("#btnSubmit").hasClass("nuevoAFN")){//crea nuevo afn
			var min = $("#inputValorMin").val(); //obtenemos valor min del input
			var max = $("#inputValorMax").val();
			if(min==max){ //en caso de que se registre un solo simbolo
				automatas.push(new AFN(min));//se genera con alfabeto de un simbolo
			} else {
				automatas.push(new AFN(min+"-"+max));//se genera con alfabeto de un rango
			}
			DibujarAFN(automatas[automatas.length-1]); //dibujamos automata
			$("#templateModal").modal("hide");//escondemos modal
			$("#inputSimbolo").val("");//reseteamos el valor del input
			//quitamos clase
			$("#btnSubmit").removeClass("nuevoAFN");
		} else if($("#btnSubmit").hasClass("unirAFN")){ //unir afn
			var afn = $("#inputGroupSelect01").val();//index de primer afn
			var afn2 = $("#inputGroupSelect02").val();//index de segundo afn
			if(afn!=afn2){//si son diferentes index
				automatas[afn].Union(automatas[afn2]); //obtenemos nuevo automata
				/*borramos automatas de la lista*/
				automatas.splice(afn2,1);
				console.log(automatas);
				DibujarAFNs(automatas);//repintamos los nuevos automatas
				/*Quitamos opciones de ambos combobox*/
				$("#inputGroupSelect01").empty().append("<option value=''>1er AFN</option>");
				$("#inputGroupSelect02").empty().append("<option value=''>2do AFN</option>");
				$("#templateModal").modal("hide");//escondemos modal
				$("#btnSubmit").removeClass("unirAFN");
				$("#btnUnirTodo").fadeOut();
			}else
				alert("Selecciona dos automatas diferentes");
		} else if($("#btnSubmit").hasClass("concatenarAFN")){//concatenar afn
			var afn = $("#inputGroupSelect01").val();//index de primer afn
			var afn2 = $("#inputGroupSelect02").val();//index de segundo afn
			if(afn!=afn2){//si son diferentes index
				automatas[afn].Concatenar(automatas[afn2]); //obtenemos nuevo automata
				/*borramos automatas de la lista*/
				automatas.splice(afn2,1);
				console.log(automatas);
				DibujarAFNs(automatas);//repintamos los nuevos automatas
				/*Quitamos opciones de ambos combobox*/
				$("#inputGroupSelect01").empty().append("<option value=''>1er AFN</option>");
				$("#inputGroupSelect02").empty().append("<option value=''>2do AFN</option>");
				$("#templateModal").modal("hide");//escondemos modal
				$("#btnSubmit").removeClass("concatenarAFN");
			}else
				alert("Selecciona dos automatas diferentes");
		} else if($("#btnSubmit").hasClass("positivaAFN")){
			var afn = $("#inputGroupSelect01").val();//id del afn seleccionado
			automatas[afn].CerraduraPositiva();//se genera cerradura al afn
			console.log(automatas);
			/*Repintamos todos los automatas*/
			DibujarAFNs(automatas);
			/*Quitamos opciones del combobox*/
			$("#inputGroupSelect01").empty().append("<option value=''>1er AFN</option>");$("#inputGroupSelect02").empty().append("<option value=''>2do AFN</option>");
			$("#templateModal").modal("hide");//escondemos modal
			$("#btnSubmit").removeClass("positivaAFN");
		} else if($("#btnSubmit").hasClass("kleeneAFN")){
			var afn = $("#inputGroupSelect01").val();//id del afn seleccionado
			automatas[afn].C_Estrella();//se genera cerradura al afn
			console.log(automatas);
			/*Repintamos todos los automatas*/
			DibujarAFNs(automatas);
			/*Quitamos opciones del combobox*/
			$("#inputGroupSelect01").empty().append("<option value=''>1er AFN</option>");$("#inputGroupSelect02").empty().append("<option value=''>2do AFN</option>");
			$("#templateModal").modal("hide");//escondemos modal
			$("#btnSubmit").removeClass("kleeneAFN");
		} else if($("#btnSubmit").hasClass("opcionalAFN")){
			var afn = $("#inputGroupSelect01").val();
			automatas[afn].Opcional();
			console.log(automatas);
			DibujarAFNs(automatas);
			/*Quitamos opciones del combobox*/
			$("#inputGroupSelect01").empty().append("<option value=''>1er AFN</option>");$("#inputGroupSelect02").empty().append("<option value=''>2do AFN</option>");
			$("#templateModal").modal("hide");//escondemos modal
			$("#btnSubmit").removeClass("opcionalAFN");
		}
	});
	/******FIN CONTROL DE BOTON DEL MODAL********************/
	/*Handler limpiar canvas*/
	$("#btnBorrar").on("click",function(){
		LimpiarContenedor();
		automatas = [];
		numEstados = 0;
		numAFN = 0;
		numTransiciones = 0;
	});

	/*Handler cuando se da click al boton de cerrar modal*/
	$("#cerrarModal").on("click",function(){
		//se debe quitar la clase agregada al btnSubmit para evitar bugs
		$("#btnSubmit").removeClass(claseBoton);
		$("#btnUnirTodo").fadeOut();
	});
	$("#cerrarModalAFD").on("click",function(){
		$("#tablaAFD thead").empty();
		$("#tablaAFD tbody").empty();
	});

	function EvaluarCadena(tablaEstados){
		var cadena = $("#inputCadena").val();
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
					console.log(tablaEstados[last_accepting_state][token_place]);
					lex.push(tablaEstados[last_accepting_state][token_place]);
					current_state = 1;
				}
			}
		}
		return lex;
	}

	/*Multiplexor que evalua que operacion del AFN se va a realizar respecto al caracter del parametro*/
	function GenerarAutomata(pila){
		var par = [] //pila para el control de parentecis;
		var afns = [] //pila para el control de afns;
		var oper = [] //pila para el control de operaciones;
		var car;
		var afn1,afn2,op,res,min,max;
		while(pila.length>0){
			car = pila.pop();
			switch(car){
				case '(':
					console.log("Parentecis abierto");
					par.push(car);
					break;
				case ')':
					console.log("Cerrar parentecis");
					if(par.length==0)
						return -1;
					par.pop();
					op = oper.pop();
					afn2 = afns.pop();
					afn1 = afns.pop();
					res = operacionDosAFNs(op,afn1,afn2);
					if(res== -1){
						alert("Error en operacion dos");
						return -1;
					}
					afns.push(res);
					break;
				case '|':
				case '&':
					console.log("operacion dos: "+car);
					oper.push(car);
					break;
				case '?':
				case '+':
				case '*':
					console.log("operacion uno: "+car);
					afn1 = afns.pop();
					res = operacionUnAFN(car,afn1);
					if(res== -1){
						alert("Error en operacion uno");
						return -1;
					}
					afns.push(res);
					break;
				case '[':
					min = pila.pop(); //obtenemos min
					pila.pop(); //quitamos coma (,)
					max = pila.pop(); //obtenemos max
					console.log("min: "+min+" max: "+max);
					pila.pop(); //quitamos "]"
					if(min>max)
						afns.push(new AFN(max+"-"+min));
					else
						afns.push(new AFN(min+"-"+max));
					break;
				case '/':
					console.log("entro: "+car);
					var car2 = pila.pop();
					console.log("Caracter especial: "+car2);
					afns.push(new AFN(car2));
					break;
				default:
					afns.push(new AFN(car));
					/*console.log("nuevo simbolo: "+car);
					if(car!='[')
						afns.push(new AFN(car));
					else{
						min = pila.pop(); //obtenemos min
						pila.pop(); //quitamos coma (,)
						max = pila.pop(); //obtenemos max
						console.log("min: "+min+" max: "+max);
						pila.pop(); //quitamos "]"
						if(min>max)
							afns.push(new AFN(max+"-"+min));
						else
							afns.push(new AFN(min+"-"+max));
					}*/
			}
		}
		//despues de vaciar la pila verificamos si no hay operaciones pendientes
		while(oper.length>0){
			/*en caso de que si, verificamos si aun hay dos afns en la pila*/
			if(afns.length>=2){
				/*en caso de que si, ahora si se hacer la operacion*/
				op = oper.pop();
				afn2 = afns.pop();
				afn1 = afns.pop();
				console.log("operacion final: "+op);
				res = operacionDosAFNs(op,afn1,afn2);
				if(res== -1){
					alert("Error en operacion dos");
					return -1;
				}
				afns.push(res);
			}
			else
				alert("Error");
		}
		/*verificamos si al final del proceso solo queda un afn en la cola de afns*/
		if(afns.length==1)
			return afns.pop();
		else{
			alert("Error quedaron mas de 1 afns al final");
			return -1;
		}
	}

	function operacionDosAFNs(op,afn1,afn2){
		switch(op){
			case '&':
				return afn1.Concatenar(afn2);
			case '|':
				return afn1.Union(afn2);
			default:
				return -1;
		}
	}

	function operacionUnAFN(op,afn){
		console.log(afn);
		console.log(op);
		switch(op){
			case '*':
				return afn.C_Estrella();
			case '+':
				return afn.CerraduraPositiva();
			case '?':
				return afn.Opcional();
			default:
				return -1;
		}
	}

	function MostrarTablaAFD(AFD){
		//generamos xcabezeras;
		var thead = $("#tablaAFD thead");
		var tbody = $("#tablaAFD tbody");
		thead.append("<tr>");
		thead.append("<th></th>");
		for(var i=0;i<AFD[0].length;i++)
			thead.append("<th scope='col'>"+AFD[0][i]+"</th>");
		thead.append("<th>Tocken</th></tr>");
		//llenamos la tabla
		for(var i=1;i<AFD.length;i++){
			tbody.append("<tr>");
			tbody.append("<td>"+i+"</td>");
			for(var j=0;j<AFD[i].length;j++){
				tbody.append("<td>"+AFD[i][j]+"</td>");
			}
			tbody.append("</tr>");
		}
	}
});


/*Constructor de modals
**claseBoton: clase que sea agrega al boton que sirve para identificar la operacion solicitada
**titulo: texto que sera el titulo del modal
**contenido: codigo html que se mostrara en el cuerpo del modal
**textoBoton: texto del boton aceptar
*/
function generarModal(titulo,contenido,textoBoton){
	$("#templateModal h5#templateModalLabel").text(titulo);
	$("#templateModal div.modal-body").html(contenido);
	$("#templateModal button#btnSubmit").text(textoBoton);
	$("#templateModal button#btnSubmit").addClass(claseBoton);
}

//creacion de cookies, esto con el objetivo de guardar la tabla AFD para usarla con las reglas gramaticales
function bake_cookie(name, value) {
  var cookie = [name, '=', JSON.stringify(value), '; domain=.', window.location.host.toString(), '; path=/;'].join('');
  document.cookie = cookie;
}

function read_cookie(name) {
 var result = document.cookie.match(new RegExp(name + '=([^;]+)'));
 result && (result = JSON.parse(result[1]));
 return result;
}

function readTextFile(file)
{
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", file, false);
    rawFile.onreadystatechange = function ()
    {
        if(rawFile.readyState === 4)
        {
            if(rawFile.status === 200 || rawFile.status == 0)
            {
                var allText = rawFile.responseText;
                alert(allText);
            }
        }
    }
    rawFile.send(null);
}