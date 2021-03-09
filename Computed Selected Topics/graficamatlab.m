clear all;
close all;

data = csvread('C:\Users\Juan Manuel\Downloads\B3-S23-4x4.csv');
uniqueEdgeList = unique(sort(data,2),'rows');
s=uniqueEdgeList(:,1); % reading its first column
dim = size(s,1);
s2 = transpose(s);
t=uniqueEdgeList(:,2); % reading its second column of csv file
t2 = transpose(t);

G = graph(s2,t2);
%remove all nodes that don't have connection
tam = numnodes(G);
id = 1;
while id <= tam
  if(size(neighbors(G,id),1)==0)
      G = rmnode(G,id);
      tam = numnodes(G);
  else
      id = id + 1;
  end
end

%showing graph
plot(G,'Layout','force');
% 
% opening a saved graphic
%openfig('C:\Users\Juan Manuel\OneDrive\Documentos\8vo semestre\Computed Selected Topics\Reporte3\Diffusion4x4.fig');