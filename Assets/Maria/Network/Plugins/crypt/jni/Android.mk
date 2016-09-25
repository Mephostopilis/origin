LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)
LOCAL_MODULE := crypt
LOCAL_MODULE_FILENAME := libcrypt
LOCAL_SRC_FILES := ./../lsha1.c \
                   ./../lua-crypt.c \
LOCAL_C_INCLUDES := $(LOCAL_PATH)/../
           
include $(BUILD_SHARED_LIBRARY)