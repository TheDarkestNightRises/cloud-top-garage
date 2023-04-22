#!/bin/bash

#Garage Service
kubectl apply -f garage-local-pvc.yaml
kubectl apply -f garage-mssql-initdb.yaml
kubectl apply -f garages-mssql-depl.yaml
kubectl apply -f garages-depl.yaml