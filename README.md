# raspi-blazor-temperature-app
The project is part of proof of concept system to verify following technologies:
* Edge to Cloud/K8s (with Raspi)
* Services hosted in K8s using Helm
* Services hosted in AKS using Helm
* Balzor SPA
* Azure Identity 
* Inner Dev Loop with Docker-Compose and MiniKube

![System Design](https://github.com/ettenauer/extension-confluent-kafka-client/tree/main/images/SystemDesign.PNG)

# Prerequisites
* Azure account to configure Azure Identity (Scope and AppRole)
* Azure account to create AKS cluster if local K8s cluster exists

# Run build immages local
## Docker-Compose
* open powershell/cmd
* navigate to docker-compose folder in repo
* docker-compose up

## MiniKube and Helm
* open powershell/cmd
* navigate to repo
* minikube start
* minikube image load ettenauer/raspi.temperature.app.server:latest (image name)
* helm install local ./helm 
* helm get manifest dev (check created yaml)
* open new shell, here two options:
** minikube service dev-raspi-temp-app
** kubectl port-forward service/local-raspi-temp-app 80:80