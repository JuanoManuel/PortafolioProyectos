class LL1{
    constructor()
    {
         var VT=[];
         var VN=[];
         var listaSimbolos=[];
         var listaReglas=[];
    }
    first(listaSimbolos){
        var conjuntoC=[];
        if(listaSimbolos[0]=='ɛ'){   
            conjuntoC.push(listaSimbolos[0]);
            return conjuntoC;
        }
        if (VT.find(function(element) {
            element=listaSimbolos[0];
          })){
              conjuntoC.push(listaSimbolos[0]);
              return conjuntoC;
        }
        
        listaReglas.forEach(element => {
            element.forEach(element => {
                element.forEach(element => {
                    conjuntoC=conjuntoC.concat(this.first(element));
                });
            });
        });
        return conjuntoC;


    }


}