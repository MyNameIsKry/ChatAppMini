"use client";

import React from 'react'
import { Users, Shield, Zap } from "lucide-react";
import { motion } from "motion/react";  

const feature = () => {
  return (
    <div className="mt-24 grid md:grid-cols-3 gap-8">
        {[
        {
            icon: Users,
            title: "Chat nhóm",
            description: "Tạo nhóm chat với bạn bè, chia sẻ khoảnh khắc và thông tin quan trọng."
        },
        {
            icon: Shield,
            title: "Bảo mật cao",
            description: "Tin nhắn được mã hóa end-to-end, đảm bảo riêng tư và an toàn tuyệt đối."
        },
        {
            icon: Zap,
            title: "Thời gian thực",
            description: "Nhận và gửi tin nhắn ngay lập tức với công nghệ SignalR hiện đại."
        }
        ].map((feature, index) => {
        const IconComponent = feature.icon;
        return (
            <motion.div
            key={index}
            initial={{ opacity: 0, y: 50 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: index * 0.25 }}
            viewport={{ once: true }}
            >
            <div className="text-center p-6 bg-white rounded-lg shadow-md">
                <IconComponent className="h-12 w-12 text-blue-600 mx-auto mb-4" />
                <h3 className="text-xl font-semibold mb-2">{feature.title}</h3>
                <p className="text-gray-600">{feature.description}</p>
            </div>
            </motion.div>
        );
        })}
    </div>
  )
}

export default feature