"use client"

import { MessageCircle, LogOut, User as UserIcon, Settings, ChevronDown } from 'lucide-react';
import Link from 'next/link';
import React, { useState, useEffect } from 'react'
import { Button } from '../ui/button';
import Image from 'next/image';
import { User } from '@/types/api';
import { motion, AnimatePresence } from 'motion/react';

export const Navigation = ({ userData }: { userData: User | null }) => {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  return (
    <>
      {isDropdownOpen && (
        <div 
          className="fixed inset-0 z-40 min-h-screen" 
          onClick={() => setIsDropdownOpen(false)}
        />
      )}
      
      <nav className="backdrop-blur-md bg-white/80 border border-white/20 rounded-2xl px-6 py-4 shadow-lg shadow-blue-500/10 relative z-50">
        <div className="flex items-center justify-between">
          <Link href="/" className="flex items-center space-x-3 group">
            <div className="relative">
              <MessageCircle className="h-8 w-8 text-blue-600 transition-transform group-hover:scale-110 group-hover:rotate-12" />
              <div className="absolute -inset-1 bg-blue-600/20 rounded-full blur opacity-0 group-hover:opacity-100 transition-opacity" />
            </div>
            <span className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent">
              ChatMini
            </span>
          </Link>

          <div className="flex items-center space-x-3">
            {userData ? (
              <div className="relative">
                <button
                  onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                  className="cursor-pointer flex items-center space-x-3 px-4 py-2 rounded-xl bg-white/50 hover:bg-white/80 transition-all duration-200 border border-white/30 hover:border-blue-200 group"
                >
                  <div className="relative">
                    <Image 
                      src={userData.avatarUrl} 
                      alt={userData.name} 
                      className="h-8 w-8 rounded-full ring-2 ring-blue-100 group-hover:ring-blue-300 transition-all" 
                      width={32} 
                      height={32} 
                    />
                    <div className="absolute -bottom-0.5 -right-0.5 h-3 w-3 bg-green-400 border-2 border-white rounded-full" />
                  </div>
                  <span className="font-medium text-gray-700 hidden sm:block">{userData.name}</span>
                  <ChevronDown className={`h-4 w-4 text-gray-500 transition-transform ${isDropdownOpen ? 'rotate-180' : ''}`} />
                </button>

                <AnimatePresence>
                  {isDropdownOpen && (
                    <motion.div
                      initial={{ opacity: 0, y: -10, scale: 0.95 }}
                      animate={{ opacity: 1, y: 0, scale: 1 }}
                      exit={{ opacity: 0, y: -10, scale: 0.95 }}
                      transition={{ duration: 0.15 }}
                      className="absolute right-0 mt-2 w-48 backdrop-blur-md bg-white/90 border border-white/20 rounded-xl shadow-xl py-2 z-50"
                    >
                      <div className="px-4 py-2 border-b border-gray-100">
                        <p className="font-medium text-gray-800">{userData.name}</p>
                        <p className="text-sm text-gray-500">{userData.email}</p>
                      </div>
                      
                      <Link href="/profile" className="flex items-center space-x-3 px-4 py-2 text-gray-700 hover:bg-blue-50 transition-colors">
                        <UserIcon className="h-4 w-4" />
                        <span>Hồ sơ</span>
                      </Link>
                      
                      <Link href="/settings" className="flex items-center space-x-3 px-4 py-2 text-gray-700 hover:bg-blue-50 transition-colors">
                        <Settings className="h-4 w-4" />
                        <span>Cài đặt</span>
                      </Link>
                      
                      <hr className="my-2 border-gray-100" />
                      
                      <button className="cursor-pointer flex items-center space-x-3 px-4 py-2 text-red-600 hover:bg-red-50 transition-colors w-full text-left">
                        <LogOut className="h-4 w-4" />
                        <span>Đăng xuất</span>
                      </button>
                    </motion.div>
                  )}
                </AnimatePresence>
              </div>
            ) : (
              <div className="flex items-center space-x-3">
                <Link href="/login">
                  <Button 
                    variant="ghost" 
                    className="cursor-pointer hover:bg-white/60 hover:text-blue-600 transition-all duration-200 border border-transparent hover:border-blue-200"
                  >
                    Đăng nhập
                  </Button>
                </Link>
                <Link href="/register">
                  <Button className="cursor-pointer bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 text-white shadow-lg hover:shadow-xl transition-all duration-200 transform hover:scale-105">
                    Đăng ký
                  </Button>
                </Link>
              </div>
            )}
            <Button className='cursor-pointer not-first:bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 text-white shadow-lg hover:shadow-xl transition-all duration-200 transform hover:scale-[1.02]'>
              <Link href="/chat">
                <span className="hidden sm:block">Chat ngay!</span>
              </Link>
            </Button>
          </div>
        </div>
      </nav>
    </>
  )
}
