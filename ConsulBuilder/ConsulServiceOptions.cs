﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsulBuilder
{
    public class ConsulServiceOptions
    {
        // 服务注册地址（Consul的地址）
        public string ConsulAddress { get; set; }

        // 服务ID
        public string ServiceId { get; set; }

        // 服务名称
        public string ServiceName { get; set; }

        // 健康检查地址
        public string HealthCheck { get; set; }

        //站点地址
        public string ServiceAddress { get; set; }
    }
}
