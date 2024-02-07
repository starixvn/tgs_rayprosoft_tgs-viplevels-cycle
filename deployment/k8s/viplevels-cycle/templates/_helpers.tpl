{{/*
Expand the name of the chart.
*/}}
{{- define "viplevels-cycle.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "viplevels-cycle.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "viplevels-cycle.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "viplevels-cycle.labels" -}}
helm.sh/chart: {{ include "viplevels-cycle.chart" . }}
{{ include "viplevels-cycle.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "viplevels-cycle.selectorLabels" -}}
app.kubernetes.io/name: {{ include "viplevels-cycle.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app: {{ include "viplevels-cycle.name" . }}
tgs.kind: viplevels
tgs.viplevels.kind: birthday
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "viplevels-cycle.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "viplevels-cycle.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
