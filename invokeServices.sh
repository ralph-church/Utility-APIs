#!/bin/bash
ENV=${APP_ENV}
echo $ENV

CANONICALNAME=${CANONICALNAME}
echo $CANONICALNAME

LOG_TRACE=$(cat /kvmnt/$ENV-LOG-TRACE)
echo $LOG_TRACE

export DatabaseSettings__ConnectionString=$(cat /kvmnt/$ENV-DB-CONNECTION-STRING)
export DatabaseSettings__DatabaseName=$(cat /kvmnt/$ENV-DATABASE-NAME)
export SeriLog__MinimumLevel__Default=$(cat /kvmnt/$ENV-LOG-MINIMUM-LEVEL-DEFAULT)
export ProducerCryptoKey=$(cat /kvmnt/$ENV-PRODUCER-CRYPTOKEY)
export TtcServicesConfig__RepairInvoiceEndPoint=$(cat /kvmnt/$ENV-TTC-SERVICES-CONFIG-REPAIR-INVOICE-ENDPOINT)
export TtcServicesConfig__AuditEndPoint=$(cat /kvmnt/$ENV-TTC-SERVICES-CONFIG-AUDIT-ENDPOINT)
export AuditEventType=$(cat /kvmnt/$ENV-AUDIT-EVENT-TYPE)

dotnet repair.service.api.dll