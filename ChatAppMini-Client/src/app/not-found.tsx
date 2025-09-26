"use client"

import Link from 'next/link';
import { MessageCircle, Home, ArrowLeft, SearchX, Wifi } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { motion } from 'motion/react';

export default function NotFound() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50 flex items-center justify-center p-4">
      <div className="max-w-2xl mx-auto text-center">
        <motion.div 
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
          className="mb-8"
        >
          <Link href="/" className="inline-flex items-center space-x-3 group">
            <div className="relative">
              <MessageCircle className="h-12 w-12 text-blue-600 transition-transform group-hover:scale-110 group-hover:rotate-12" />
              <div className="absolute -inset-1 bg-blue-600/20 rounded-full blur opacity-0 group-hover:opacity-100 transition-opacity" />
            </div>
            <span className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent">
              ChatMini
            </span>
          </Link>
        </motion.div>

        <motion.div
          initial={{ opacity: 0, scale: 0.5 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.6, delay: 0.2 }}
          className="mb-8"
        >
          <div className="relative">
            <h1 className="text-9xl font-bold bg-gradient-to-r from-blue-600 via-indigo-600 to-purple-600 bg-clip-text text-transparent mb-4">
              404
            </h1>
            {/* Floating elements */}
            <motion.div
              animate={{ y: [-10, 10, -10] }}
              transition={{ duration: 3, repeat: Infinity, ease: "easeInOut" }}
              className="absolute top-4 -right-8"
            >
              <SearchX className="h-8 w-8 text-blue-400 opacity-70" />
            </motion.div>
            <motion.div
              animate={{ y: [10, -10, 10] }}
              transition={{ duration: 2.5, repeat: Infinity, ease: "easeInOut" }}
              className="absolute top-8 -left-12"
            >
              <Wifi className="h-6 w-6 text-indigo-400 opacity-60" />
            </motion.div>
          </div>
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6, delay: 0.4 }}
          className="backdrop-blur-md bg-white/90 border border-white/20 rounded-2xl p-8 shadow-xl mb-8"
        >
          <h2 className="text-3xl font-bold bg-gradient-to-r from-gray-800 to-gray-600 bg-clip-text text-transparent mb-4">
            Oops! Trang không tồn tại
          </h2>
          <p className="text-lg text-gray-600 mb-6 max-w-md mx-auto">
            Có vẻ như trang bạn đang tìm kiếm đã biến mất hoặc không tồn tại. 
            Đừng lo lắng, hãy quay lại trang chủ và tiếp tục cuộc trò chuyện!
          </p>

          <div className="flex flex-col sm:flex-row gap-4 justify-center items-center">
            <Link href="/">
              <Button className="bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 text-white shadow-lg hover:shadow-xl transition-all duration-200 transform hover:scale-105 px-8 py-3">
                <Home className="h-5 w-5 mr-2" />
                Về trang chủ
              </Button>
            </Link>
            
            <Button 
              variant="ghost" 
              onClick={() => window.history.back()}
              className="hover:bg-white/60 hover:text-blue-600 transition-all duration-200 border border-transparent hover:border-blue-200 px-8 py-3"
            >
              <ArrowLeft className="h-5 w-5 mr-2" />
              Quay lại
            </Button>
          </div>
        </motion.div>

        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 0.6, delay: 0.6 }}
          className="text-center"
        >
          <p className="text-sm text-gray-500 mb-2">
            Có thể bạn đang tìm kiếm:
          </p>
          <div className="flex flex-wrap justify-center gap-2">
            <Link href="/login" className="text-blue-600 hover:text-indigo-600 text-sm font-medium hover:underline transition-colors">
              Đăng nhập
            </Link>
            <span className="text-gray-300">•</span>
            <Link href="/register" className="text-blue-600 hover:text-indigo-600 text-sm font-medium hover:underline transition-colors">
              Đăng ký
            </Link>
            <span className="text-gray-300">•</span>
            <Link href="/chat" className="text-blue-600 hover:text-indigo-600 text-sm font-medium hover:underline transition-colors">
              Chat
            </Link>
          </div>
        </motion.div>

        <motion.div
          animate={{ rotate: 360 }}
          transition={{ duration: 20, repeat: Infinity, ease: "linear" }}
          className="absolute top-20 left-20 opacity-10"
        >
          <MessageCircle className="h-24 w-24 text-blue-600" />
        </motion.div>
        
        <motion.div
          animate={{ rotate: -360 }}
          transition={{ duration: 25, repeat: Infinity, ease: "linear" }}
          className="absolute bottom-20 right-20 opacity-10"
        >
          <MessageCircle className="h-16 w-16 text-indigo-600" />
        </motion.div>
      </div>
    </div>
  );
}