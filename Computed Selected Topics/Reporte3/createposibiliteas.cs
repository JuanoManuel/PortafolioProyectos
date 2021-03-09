private void CreatePosibilities(int n)
        {
            ulong numCombinaciones = (ulong)Math.Pow(2.0, n * n);
            Debug.WriteLine("Creando todos los caminos de " + n + "x" + n);
            transicion.Clear();
            recurrencias.Clear();
            clasificados.Clear();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text File|*.txt";
                saveFileDialog.Title = "Historial de nodos";
                saveFileDialog.ShowDialog();
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                for (ulong comb = 1; comb < numCombinaciones; comb++)
                {
                    Debug.WriteLine("LLenando a historia");
                    string cadBinaria = Convert.ToString((long)comb, 2);
                    //rellenamos de 0 para complir con el numero de celulas de la matriz
                    while (cadBinaria.Length < (n * n)) cadBinaria = 0 + cadBinaria;
                    int[][] aux = StringToMatrix(cadBinaria);
                    int[][] bufferMatrix = GameOfLife(aliveConditions, surviveConditions, aux);
                    string strBuffer = MatrixToString(bufferMatrix);
                    ulong nextValue = Convert.ToUInt64(strBuffer, 2);
                    writer.WriteLine(comb + " = " + cadBinaria + " --> " + nextValue + " = " +strBuffer);
                    Debug.WriteLine(comb + " = " + cadBinaria + " --> " + nextValue + " = " + strBuffer);
                    transicion.Add(comb, nextValue);
                }

                writer.Dispose();
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al exportar el grado" + e.Message);
            }
            
            //despues de que obtenemos todos los posibles caminos, toca ordenarlos y obtener los que se repiten
            //el diccionario recurrencias tiene un key que es el valor entero de la matriz generada
            //y el value es una lista de todos los nodos que llevan a ese valor entero
            foreach (var item in transicion.OrderBy(key => key.Value))
            {
                if (recurrencias.ContainsKey(item.Value))
                {
                    recurrencias[item.Value].Add(item.Key);
                }
                else
                {
                    List<ulong> aux = new List<ulong>();
                    aux.Add(item.Key);
                    recurrencias.Add(item.Value, aux);
                }
            }
            //Debug.WriteLine(ExportarGrafo(recurrencias));
            Debug.WriteLine("Exportando datos no filtrados");
            ExportarGrafoAMatlab(recurrencias,"Guardar no filtrados");
            //ahora que tenemos las recurrencias de todos los nodos se intentara generar el grafo con los caminos filtrados
            //de todas las recurrencias se generaran sus respectivos grafos
            foreach (KeyValuePair<ulong, List<ulong>> item in recurrencias)
            {
                Dictionary<ulong, List<ulong>> aux = new Dictionary<ulong, List<ulong>>();
                aux.Add(item.Key, item.Value);
                //reccoremos todos los elementos de la lista almacenada en el value de item
                for (int i = 0; i < item.Value.Count; i++)
                {
                    ulong auxLista = item.Value[i];
                    if (recurrencias.ContainsKey(auxLista))
                    {
                        if (!aux.ContainsKey(auxLista))
                            aux.Add(auxLista, recurrencias[auxLista]);
                    }
                }

                Graph actualGraph = new Graph(aux);
                string key = actualGraph.getKey();
                if (aux.Count > 0 && !clasificados.ContainsKey(key))
                    clasificados.Add(key, actualGraph);
            }
            //para los filtrados tenemos que juntar todas los diccionarios de cada uno de los graphs en uno solo
            Dictionary<ulong, List<ulong>> auxDic = new Dictionary<ulong, List<ulong>>();
            foreach(KeyValuePair<string,Graph> itemClas in clasificados)
            {
                foreach(KeyValuePair<ulong,List<ulong>> itemGraph in itemClas.Value.getAllNodes())
                {
                    auxDic[itemGraph.Key] = itemGraph.Value;
                }
            }
            ExportarGrafoAMatlab(auxDic,"Guardar filtrados");
            Debug.WriteLine("Termino de generar todos los caminos");
        }

        private void ExportarGrafoAMatlab(Dictionary<ulong, List<ulong>> dic,string titulo)
        {
            Debug.WriteLine("Comenzando exportacion: "+titulo);
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text File|*.csv";
                saveFileDialog.Title = titulo;
                saveFileDialog.ShowDialog();
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                //recorremos todo el diccionario
                foreach (KeyValuePair<ulong, List<ulong>> item in dic)
                {
                    //recorremos la lista del value para crear cada uno de los nodos y conectarlo al nodo de key
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        writer.WriteLine((item.Value[i]+1)+","+(item.Key+1));
                    }
                }
                writer.Dispose();
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al exportar el grado" + e.Message);
            }
            Debug.WriteLine("Exportacion con exito");
            MessageBox.Show("Exportacion " + titulo + " realizada con exito");
        }

        private string MatrixToString(int[][] matrix)
        {
            string res = "";
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    res += matrix[i][j] == 1 ? "1" : "0";
            return res;
        }

        private int[][] StringToMatrix(string cad)
        {
            int size = Convert.ToInt32(Math.Sqrt(cad.Length));
            int[][] newMatrix = new int[size][];
            for(int i = 0,index = 0; i < size; i++)
            {
                newMatrix[i] = new int[size];
                for(int j = 0; j < size; j++)
                {
                    newMatrix[i][j] = cad[index++] == '1' ? 1 : 0;
                }
            }

            return newMatrix;
        }