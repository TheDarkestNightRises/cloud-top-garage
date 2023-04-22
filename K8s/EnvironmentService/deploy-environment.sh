#!/bin/bash

#Environment service
kubectl apply -f environment-mssql.yaml
kubectl apply -f environment-local-pvc.yaml 
kubectl apply -f environment-mssql-initdb.yaml
kubectl apply -f environments-mssql-depl.yaml
kubectl apply -f environments-depl.yaml